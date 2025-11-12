CREATE DATABASE simulador;
USE simulador;

-- tabela para configurações/snapshots
CREATE TABLE configuracoes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100),
    data_criacao DATETIME DEFAULT CURRENT_TIMESTAMP,
    quantidade_corpos INT,
    iteracoes INT,
    intervalo_ms INT
);

-- tabela de corpos vinculada à configuração (estado)
CREATE TABLE corpos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    configuracao_id INT,
    nome VARCHAR(100),
    massa DOUBLE,
    densidade DOUBLE,
    posx DOUBLE,
    posy DOUBLE,
    velx DOUBLE,
    vely DOUBLE,
    raio DOUBLE,
    FOREIGN KEY (configuracao_id) REFERENCES configuracoes(id) ON DELETE CASCADE
);

-- tabela para iterações (registro do tempo / passo)
CREATE TABLE iteracoes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    passo INT,
    data_hora DATETIME DEFAULT CURRENT_TIMESTAMP,
    observacao VARCHAR(255)
);

-- ligação muitos-para-muitos iteracao <-> corpo_estado (estado dos corpos em cada iteração)
CREATE TABLE iteracao_corpo (
    iteracao_id INT,
    corpo_id INT,
    massa DOUBLE,
    densidade DOUBLE,
    posx DOUBLE,
    posy DOUBLE,
    velx DOUBLE,
    vely DOUBLE,
    raio DOUBLE,
    PRIMARY KEY (iteracao_id, corpo_id),
    FOREIGN KEY (iteracao_id) REFERENCES iteracoes(id) ON DELETE CASCADE
);
