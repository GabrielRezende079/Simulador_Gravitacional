using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrabalhoV01
{
    public class PersistenciaMySql
    {
        private readonly string connStr = "Server=localhost;Database=simulador;Uid=root;Pwd=C@rros079;";

        public void SalvarConfiguracao(List<Corpo> corpos, int iteracoes, int intervalo)
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var tx = conn.BeginTransaction();

            var cmdCfg = new MySqlCommand(
                "INSERT INTO configuracoes (nome, quantidade_corpos, iteracoes, intervalo_ms) VALUES (@n, @q, @it, @ms); SELECT LAST_INSERT_ID();",
                conn, tx);
            cmdCfg.Parameters.AddWithValue("@n", "Config_" + DateTime.Now.ToString("HHmmss"));
            cmdCfg.Parameters.AddWithValue("@q", corpos.Count);
            cmdCfg.Parameters.AddWithValue("@it", iteracoes);
            cmdCfg.Parameters.AddWithValue("@ms", intervalo);

            long cfgId = Convert.ToInt64(cmdCfg.ExecuteScalar());

            // Cria uma cópia da lista para evitar modificações durante iteração
            var corposCopia = corpos.ToList();

            foreach (var c in corposCopia)
            {
                var cmd = new MySqlCommand(
                    "INSERT INTO corpos (configuracao_id, corpo_id, nome, massa, densidade, posx, posy, velx, vely, raio) VALUES (@cfg, @cid, @n, @m, @d, @x, @y, @vx, @vy, @r)",
                    conn, tx);
                cmd.Parameters.AddWithValue("@cfg", cfgId);
                cmd.Parameters.AddWithValue("@cid", c.Id);
                cmd.Parameters.AddWithValue("@n", c.Nome);
                cmd.Parameters.AddWithValue("@m", c.Massa);
                cmd.Parameters.AddWithValue("@d", c.Densidade);
                cmd.Parameters.AddWithValue("@x", c.PosX);
                cmd.Parameters.AddWithValue("@y", c.PosY);
                cmd.Parameters.AddWithValue("@vx", c.VelX);
                cmd.Parameters.AddWithValue("@vy", c.VelY);
                cmd.Parameters.AddWithValue("@r", c.Raio);
                cmd.ExecuteNonQuery();
            }
            tx.Commit();
        }

        public void SalvarIteracao(List<Corpo> corpos, int passo)
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var tx = conn.BeginTransaction();

            var cmdIt = new MySqlCommand("INSERT INTO iteracoes (passo) VALUES (@p); SELECT LAST_INSERT_ID();", conn, tx);
            cmdIt.Parameters.AddWithValue("@p", passo);
            long idIter = Convert.ToInt64(cmdIt.ExecuteScalar());

            // CRÍTICO: Cria uma cópia da lista para evitar "Collection was modified"
            var corposCopia = corpos.ToList();

            foreach (var c in corposCopia)
            {
                var cmd = new MySqlCommand(
                    "INSERT INTO iteracao_corpo (iteracao_id, corpo_id, massa, densidade, posx, posy, velx, vely, raio) VALUES (@i, @cid, @m, @d, @x, @y, @vx, @vy, @r)",
                    conn, tx);
                cmd.Parameters.AddWithValue("@i", idIter);
                cmd.Parameters.AddWithValue("@cid", c.Id);
                cmd.Parameters.AddWithValue("@m", c.Massa);
                cmd.Parameters.AddWithValue("@d", c.Densidade);
                cmd.Parameters.AddWithValue("@x", c.PosX);
                cmd.Parameters.AddWithValue("@y", c.PosY);
                cmd.Parameters.AddWithValue("@vx", c.VelX);
                cmd.Parameters.AddWithValue("@vy", c.VelY);
                cmd.Parameters.AddWithValue("@r", c.Raio);
                cmd.ExecuteNonQuery();
            }

            tx.Commit();
        }

        public List<Corpo> CarregarUltimaConfiguracao()
        {
            var corpos = new List<Corpo>();
            using var conn = new MySqlConnection(connStr);
            conn.Open();

            string sqlCfg = "SELECT id FROM configuracoes ORDER BY id DESC LIMIT 1";
            var cmdCfg = new MySqlCommand(sqlCfg, conn);
            var resultCfg = cmdCfg.ExecuteScalar();

            if (resultCfg == null)
                return corpos;

            long cfgId = Convert.ToInt64(resultCfg);

            string sql = "SELECT corpo_id, nome, massa, densidade, posx, posy, velx, vely FROM corpos WHERE configuracao_id=@cfg ORDER BY corpo_id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@cfg", cfgId);
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                corpos.Add(new Corpo(
                    rdr.GetInt32(0),    // corpo_id
                    rdr.GetString(1),   // nome
                    rdr.GetDouble(2),   // massa
                    rdr.GetDouble(3),   // densidade
                    rdr.GetDouble(4),   // posx
                    rdr.GetDouble(5),   // posy
                    rdr.GetDouble(6),   // velx
                    rdr.GetDouble(7)    // vely
                ));
            }
            return corpos;
        }
    }
}