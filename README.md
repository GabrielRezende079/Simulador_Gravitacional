# 🪐 Simulador Gravitacional – C# (WinForms + Física + MySQL + Paralelismo)

Este projeto é um simulador gravitacional 2D desenvolvido em C# com Windows Forms, que modela corpos celestes interagindo através da força gravitacional de Newton. Cada corpo possui massa, densidade, raio, posição e velocidade — e a simulação atualiza tudo passo a passo usando um timer e cálculos físicos.

<img width="1286" height="778" alt="Image" src="https://github.com/user-attachments/assets/888b6d2e-48a3-4110-8ce1-3a86b427731b" />

## ✨ Funcionalidades

- ✔ Gravação e leitura de configurações iniciais
- ✔ Salvamento automático de cada iteração
- ✔ Banco MySQL completo para registrar tudo
- ✔ Processamento paralelo (PLINQ / Parallel.For)
- ✔ Sistema de colisão com absorção (corpos se fundem)
- ✔ Painel gráfico com renderização simples dos corpos

## 🎯 Objetivo do Projeto

Criar um simulador físico simples, porém robusto, capaz de:

- Gerar corpos aleatórios ou carregados via arquivo/banco
- Simular gravidade, movimento e colisões
- Registrar cada iteração da simulação
- Gravar e carregar estados no MySQL
- Possibilitar experimentação acadêmica de algoritmos e simulações

Este projeto foi desenvolvido como trabalho da disciplina de Programação Avançada / Sistemas, envolvendo conceitos de OO, física, bancos relacionais e processamento paralelo.

## 🧩 Arquitetura do Sistema

### Estrutura principal

```
Form1.cs             → Interface e controles
Universo.cs          → Lógica da simulação física
Corpo.cs             → Modelo dos corpos
PersistenciaTxt.cs   → Persistência em arquivos .txt
PersistenciaMySql.cs → Persistência no MySQL
```

## 🧪 Funcionamento da Simulação

### 🔢 1. Força de gravidade

Para cada par de corpos:

```
F = G · (m₁m₂) / d²
```

A aceleração aplicada:

```
aₓ = (F · dx) / (d · m₁)
aᵧ = (F · dy) / (d · m₁)
```

### 🪐 2. Atualização de movimento

```
vₓ = vₓ + aₓ
vᵧ = vᵧ + aᵧ
x = x + vₓ
y = y + vᵧ
```

### 💥 3. Colisões com absorção

Se a distância entre centros for menor que a soma dos raios:

- O maior absorve o menor
- Conservação de momento é aplicada

```
vf = (m₁v₁ + m₂v₂) / (m₁ + m₂)
```

- Massa é somada
- Raio é recalculado:

```
raio = k · √massa
```

### 🏎 4. Paralelismo

A atualização dos corpos usa:

- `Parallel.For`
- PLINQ (`AsParallel`)

Isso aumenta desempenho quando existem 100+ corpos.

## 🗄 Banco de Dados MySQL

### Estrutura utilizada (já testada)

- `configuracoes` (snapshot inicial)
- `corpos` (estado inicial dos corpos)
- `iteracoes` (registro de cada passo da simulação)
- `iteracao_corpo` (estado dos corpos em cada passo)

<img width="1536" height="1024" alt="Image" src="https://github.com/user-attachments/assets/7d95829a-8ffb-4892-9631-41d748749b60" />

### Como funciona

**Salvar Atual (btn3)**
- Salva uma "configuração" no MySQL
- Grava todos os corpos daquela configuração
- Fica armazenado como snapshot inicial

**Salvar Iterações (btn4)**
- Você escolhe um arquivo TXT
- Cada iteração gravada também vai para o MySQL
- E uma cópia simples é escrita no arquivo escolhido

**Carregar (btn5)**
- Abre uma configuração já gravada
- Reconstrói todos os corpos envolvidos

## 🖥 Interface – Botões

| Botão | Descrição |
|-------|-----------|
| **Gerar corpos aleatórios** | Cria N corpos com massa, densidade, posição e velocidade aleatória |
| **Iniciar simulação** | Ativa o timer e começa a aplicação das forças |
| **Salvar Atual** | Salva o estado atual no TXT e no MySQL |
| **Salvar Iterações** | Escolhe um arquivo e passa a logar cada iteração |
| **Carregar configuração** | Abre um snapshot salvo e reconstroi o universo |

## 📦 Estrutura dos arquivos TXT gravados

### Configuração inicial (btn3)

```
# CONFIG
Iteracoes=1000
Intervalo=20
Quantidade=200

Corpo0;massa;densidade;posX;posY;velX;velY;raio
Corpo1;...
```

### Iterações (btn4)

```
ITERACAO 1
Corpo0;x;y;vx;vy;massa;raio
Corpo1;...

ITERACAO 2
Corpo0;...
```

## ⚙ Requisitos para rodar

### Windows

- Visual Studio 2022
- .NET Framework 4.7+ ou .NET 6 (dependendo do template)
- MySQL 8.x
- Biblioteca oficial `MySql.Data`

### Banco

Crie o banco de dados:

```sql
CREATE DATABASE simulador;
```

E execute as tabelas fornecidas.

## 🚀 Como Executar

1. Clone o repositório
2. Abra no Visual Studio
3. Ajuste o MySQL em `PersistenciaMySql.cs`:

```csharp
Server=localhost;Database=simulador;Uid=root;Pwd=;
```

4. Rode o projeto
5. Clique em **Gerar** → **Iniciar**
6. Experimente salvar e carregar estados

---

Desenvolvido com ❤️ para fins acadêmicos
