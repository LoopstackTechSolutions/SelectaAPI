create database dbSelecta;
use dbSelecta;

create table if not exists tbCategoria
(
IdCategoria int primary key auto_increment,
Nome varchar(50) not null
);

create table if not exists tbFuncionario
(
IdFuncionario int primary key auto_increment,
Nome varchar(50) not null,
Email varchar(50) not null unique,
Senha varchar(50) not null,
Cpf char(11) not null unique,
NivelAcesso varchar(30) -- "comum"/"gerente"
);

alter table tbFuncionario modify column Senha varchar(255) not null;

create table if not exists tbHistorico
(
IdHistorico int primary key auto_increment,
IdRegistro int not null,
NomeTabela varchar(50) not null,
DataOperacao datetime default current_timestamp,
Operacao varchar(20) not null,
NovoValor text not null,
AntigoValor text
);

create table if not exists tbCliente
(
IdCliente int primary key auto_increment,
Nome varchar(50) not null,
Email varchar(50) not null,
Senha varchar(50) not null,
Saldo decimal(10,2) default 0
);

alter table tbCliente modify column Senha varchar(255) not null;

create table if not exists tbCategoria_Cliente
(
IdCategoria int not null,
IdCliente int not null,
foreign key (IdCategoria) references tbCategoria (IdCategoria),
foreign key (IdCliente) references tbCliente (IdCliente),
primary key (IdCliente, IdCategoria)
);

create table if not exists tbCartao
(
IdCartao int primary key auto_increment,
IdCliente int not null,
NumeroCartao char(16) not null,
Bandeira varchar(30) not null,
Modalidade boolean not null, -- true = credito / false = debito
DataValidade datetime not null,
NomeTitular varchar(50) not null,
foreign key (IdCliente) references tbcliente (idcliente)
);

create table if not exists tbEndereco
(
IdEndereco int primary key auto_increment,
Cep int unique not null,
IdCliente int,
Logradouro varchar(100) not null,
isPrincipal boolean default true,
foreign key (IdCliente) references tbCliente (IdCliente)
);

create table if not exists tbProduto
(
IdProduto int primary key auto_increment,
IdVendedor int not null,
Nome varchar(50) not null,
Descricao varchar(1000),
Quantidade int,
Condicao boolean not null,  -- true = "novo" / false = "usado"
PrecoUnitario decimal(10,2) not null,
Peso int not null, -- em gramas
`Status` varchar(40), -- "ativo"/"inativo"/"suspenso"/"disponível",
Nota decimal(2,1) default 0, 
foreign key (IdVendedor) references tbCliente (IdCliente)
);

create table if not exists tbCategoria_Produto
(
IdCategoria int,
IdProduto int,
foreign key (IdCategoria) references tbCategoria (IdCategoria),
foreign key (IdProduto) references tbProduto (IdProduto),
primary key (IdCategoria, IdProduto)
);

create table if not exists tbCentro_Distribuicao
(
IdCD int primary key auto_increment,
IdEndereco int not null,
Nome varchar(50) not null,
foreign key (IdEndereco) references tbEndereco (IdEndereco)
);

create table if not exists tbEmpresa_Parceira
(
IdEmpresa int primary key,
Cnpj decimal(16,0) not null,
Nota decimal(3,1) default 0,
TaxaFrete int,
foreign key (IdEmpresa) references tbCliente (IdCliente)
);

create table if not exists tbPromocao
(
IdPromocao int primary key auto_increment,
IdProduto int not null,
`Status` varchar(50) default "ativa", -- "ativa"/"inativa"
Desconto int not null, -- em porcentagem
ValidaAte date,
foreign key (IdProduto) references tbProduto (IdProduto)
);

create table if not exists tbNotificacao
(
IdNotificacao int primary key auto_increment,
Mensagem varchar(200) not null,
TabelaContexto varchar(50) not null
);

create table if not exists tbNotificacao_Cliente
(
IdNotificacaoCliente int primary key auto_increment,
IdNotificacao int not null,
IdCliente int not null,
IdContexto int not null,
DataCriacao datetime default current_timestamp,
IsLida boolean default false,
foreign key (IdNotificacao) references tbNotificacao(IdNotificacao),
foreign key (IdCliente) references tbCliente(IdCliente)
);

create table if not exists tbLista_Desejo
(
IdProduto int not null,
IdCliente int not null,
foreign key (IdProduto) references tbProduto(IdProduto),
foreign key (IdCliente) references tbCliente(IdCliente)
);

create table if not exists tbCarrinho
(
IdProduto int primary key,
IdCliente int not null,
Quantidade int not null,
foreign key (IdProduto) references tbProduto(IdProduto),
foreign key (IdCliente) references tbCliente(IdCliente)
);

create table if not exists tbReview
(
IdReview int primary key auto_increment,
IdCliente int not null,
IdProduto int not null,
Nota tinyint not null, -- 1 a 5 estrelas
Comentario varchar(200) not null,
foreign key (IdCliente) references tbCliente(IdCliente),
foreign key (IdProduto) references tbProduto(IdProduto)
);

create table if not exists tbTicket
(
IdTicket int primary key auto_increment,
IdCliente int not null,
IdFuncionario int,
`Status` varchar(30) default "pendente", -- "pendente"/"ativo"/"arquivado"
foreign key (IdCliente) references tbCliente(IdCliente),
foreign key (IdFuncionario) references tbFuncionario(IdFuncionario)
);

create table if not exists tbMensagem
(
IdMensagem int primary key auto_increment,
IdTicket int not null,
Mensagem text not null,
DataCriacao datetime default current_timestamp,
Remetente varchar(50) not null, -- "cliente"/"funcionario"
IdRemetente int not null,
foreign key (IdTicket) references tbTicket (IdTicket)
);

create table if not exists tbPedido
(
IdPedido int primary key auto_increment,
IdComprador int not null,
IdEnderecoEntrega int not null,
DataPedido datetime default current_timestamp,
Total decimal(10,2),
Frete decimal(5,2),
StatusPagamento varchar(20) default "pendente", -- "pago"/"pendente"
foreign key (IdEnderecoEntrega) references tbEndereco(IdEndereco),
foreign key (IdComprador) references tbCliente (IdCliente)
);

create table if not exists tbPagamento
(
IdPagamento int primary key auto_increment,
IdPedido int not null,
IdCartao int,
DataPagamento datetime default current_timestamp,
FormaPagamento varchar(30) not null, -- "debito"/"credito"/"pix"
foreign key (IdPedido) references tbPedido (IdPedido),
foreign key (IdCartao) references tbCartao (IdCartao)
);

create table if not exists tbProduto_Pedido
(
IdProduto int not null,
IdPedido int not null,
Quantidade int not null,
Valor decimal(10,2) not null,
Frete decimal(5,2) not null,
`Status` varchar(30) default "pendente", -- "entregue"/"em transito"/"pendente"
TipoEntrega boolean not null, -- true = entrega própria / false = entrega selecta
foreign key (IdProduto) references tbProduto(IdProduto),
foreign key (IdPedido) references tbPedido(IdPedido),
primary key (IdProduto, IdPedido)
);

create table if not exists tbPedido_Interno
(
IdPedidoInterno int primary key auto_increment,
IdProduto int not null,
Quantidade int not null,
IdEmpresa int not null,
IdFuncionario int not null,
IdCD int not null,
`Status` varchar(50) default "pendente", -- "pendente"/"entregue"
foreign key (IdProduto) references tbProduto(IdProduto),
foreign key (IdEmpresa) references tbEmpresa_Parceira(IdEmpresa),
foreign key (IdFuncionario) references tbFuncionario(IdFuncionario),
foreign key (IdCD) references tbCentro_Distribuicao (IdCD)
);

create table if not exists tbProduto_Cd
(
IdCD int not null,
IdProduto int not null,
Quantidade int not null,
foreign key (IdCD) references tbCentro_Distribuicao(IdCD),
foreign key (IdProduto) references tbProduto(IdProduto)
);

create table if not exists tbEntrega
(
IdEntrega int primary key auto_increment,
IdPedido int,
IdPedidoInterno int,
`Status` varchar(30) default "despachada", -- "despachada"/"entregue"
Remetente varchar(20) not null, -- "cd"/"vendedor"/"empresa parceira"
IdRemetente int not null,
Destinatario varchar(20) not null, -- "cd"/"comprador"
IdDestinatario int,
foreign key (IdPedido) references tbPedido(IdPedido),
foreign key (IdPedidoInterno) references tbPedido_Interno(IdPedidoInterno)
);

create table if not exists tbEntrega_Produto
(
IdEntrega int,
IdProduto int,
foreign key (IdEntrega) references tbEntrega (IdEntrega),
foreign key (IdProduto) references tbProduto (IdProduto),
primary key (IdEntrega, IdProduto)
);

create table if not exists tbEntregador
(
IdEntregador int primary key,
IdEndereco int not null,
Cnh char(11) not null,
Eligibilidade boolean default false,
foreign key (IdEndereco) references tbEndereco (IdEndereco),
foreign key (IdEntregador) references tbCliente (IdCliente)
);

create table if not exists tbVendedor
(
IdVendedor int primary key,
Nota decimal(3,1) default 0,
TaxaFrete int,
foreign key (IdVendedor) references tbCliente (IdCliente)
);

create table if not exists tbEntrega_Final
(
IdEntrega int not null,
IdEntregador int not null,
foreign key (IdEntrega) references tbEntrega(IdEntrega),
foreign key (IdEntregador) references tbEntregador(IdEntregador)
);

create table if not exists tbImagemProduto
(
IdImagem int primary key auto_increment,
IdProduto int not null,
S3Key varchar(512) not null unique,
IsPrincipal boolean not null, -- false = não é principal / true = é principal
foreign key (IdProduto) references tbProduto (IdProduto)
);

create table if not exists tbPergunta
(
IdPergunta int primary key auto_increment,
IdCliente int not null,
IdProduto int not null,
Pergunta varchar(250) not null,
Resposta varchar(250),
foreign key (IdCliente) references tbCliente (IdCliente),
foreign key (IdProduto) references tbProduto (IdProduto)
);

-- -------------------------------------------------------POPULANDO AS TABELAS------------------------------------------------



INSERT INTO tbCategoria (nome) VALUES
('Eletrônicos e Tecnologias'), ('Celulares'), ('Acessórios para Celulares'), ('Computadores'), ('Notebooks'), ('Tablets'),
('TVs e Áudio'), ('Eletrodomésticos'), ('Moda e Vestuário'), ('Roupas Femininas'), ('Roupas Masculinas'), ('Roupas Infantis'),
('Tênis'), ('Sandálias e Chinelos'), ('Bolsas e Mochilas'), ('Relógios'), ('Joias e Bijuterias'),('Casa, Móveis e Jardim'),
('Decoração'), ('Iluminação'), ('Móveis para Sala'), ('Móveis para Quarto'), ('Cozinha e Utensílios'), ('Cama, Mesa e Banho'),
('Jardim e Ferramentas'), ('Beleza e Cuidados Pessoais'), ('Maquiagem'), ('Perfumes'), ('Cabelos'), ('Cuidados com a Pele'),
('Barbearia e Cuidados Masculinos'), ('Alimentos e Bebidas'), ('Frutas e Verduras'), ('Carnes e Frios'), ('Bebidas Não Alcoólicas'), ('Vinhos'),
('Cervejas'), ('Snacks e Doces'), ('Esportes e Fitness'), ('Bicicletas'), ('Futebol'), ('Corrida'),
('Natação'), ('Academia e Musculação'), ('Yoga e Pilates'), ('Bebês, Crianças e Maternidade'), ('Carrinhos de Bebê'), ('Brinquedos'),
('Fraldas e Higiene'), ('Roupas para Bebê'), ('Saúde e Bem-Estar'), ('Vitaminas e Suplementos'), ('Primeiros Socorros'), ('Equipamentos Médicos'),
('Automotivo e Industrial'), ('Peças para Carros'), ('Acessórios para Carros'), ('Motos'), ('Pneus e Rodas'), ('Animais de Estimação'),
('Rações'), ('Petiscos'), ('Camas e Arranhadores'), ('Brinquedos para Pets'), ('Aquarismo'), ('Ferramentas e Construção'),
('Ferramentas Elétricas'), ('Ferramentas Manuais'), ('Materiais de Construção'), ('Tintas'),('Segurança do Trabalho'), ('Lazer, Hobbies e Entretenimento'),
('Livros'), ('Videogames'), ('Jogos de Tabuleiro'), ('Instrumentos Musicais'), ('Artesanato'), ('Papelaria, Escritório e Escola'),
('Materiais Escolares'), ('Impressoras e Suprimentos'), ('Móveis de Escritório'), ('Viagem e Bagagem'),('Malas'), ('Mochilas de Viagem'),
('Acessórios de Viagem'), ('Eventos e Festas'), ('Artigos para Casamento'), ('Decoração de Festas'), ('Fantasias'), ('Decoração Sazonal');

INSERT INTO tbFuncionario (nome, email, senha, cpf, nivelacesso) VALUES
("Júlia Amorim", "julia.amorim@selecta.com.br", "12345678", 55555555555, "comum"),
("Roberto Cunha", "roberto.cunha@selecta.com.br", "12345678", 66666666666, "gerente");

INSERT INTO tbCliente (nome, email, senha) values
("Gilmar Júnior", "gjunior6813@gmail.com", "12345678"),
("Bianca Guimarães", "biancaguimaraes@yahoo.com.br", "12345678"),
("Lucas Tobias", "tobiastobiastobias@uol.com.br", "12345678"),
("Márcia Cruz", "marcinhaxd@gmail.com", "12345678"),
("Calçados Sérgio", "calcados.sergio@calcadossergio.com.br", "12345678"),
("Kabum", "kabum@kabum.org.br", "12345678"),
("Loja Vende Tudo", "contato@vendetudo.org.br", "12345678");

insert into tbCategoria_Cliente (IdCategoria, IdCliente) values
(72, 1),(73, 1),(1, 1),(4, 1),(5, 1),(9, 1),(10, 1),(11, 1),(3, 1),(7, 1),
(1, 2),(2, 2),(4, 2),(5, 2),(23, 2),(39, 2),(42, 2),(44, 2),(85, 2),(13, 2),
(14, 3),(8, 3),(9, 3),(10, 3),(11, 3),(12, 3),(1, 3),(4, 3),(3, 3),(7, 3);

insert into tbEndereco (cep, idcliente, logradouro) values
(05313020, 1, "Rua Othão, 145 – Vila Leopoldina, São Paulo"),
(07140330, 1, "Rua Monsenhor Paulo, 288 – Jardim Marilena, Guarulhos"),
(06288010, 2, "Rua Sabirigui, 67 – Vila Menck, Osasco"),
(06114140, 2, "Rua Joaquim Israel de Macedo, 512 – Pestana, Osasco"),
(09060310, 3, "Rua José Lins do Rego, 379 – Vila Valparaíso, Santo André"),
(04425120, 4, "Rua Nancy Larsen, 84 – Guacuri, São Paulo"),
(04735000, NULL, 'Rua São Benedito, 529 – Santo Amaro, São Paulo'),
(06462470, NULL, 'Rua Três, 470 – Parque Imperial, Barueri'),
(09111680, NULL, 'Rua Amazonas, 120 – Cidade São Jorge, Santo André'),
(06170200, NULL, 'Rua João José da Silva, 220 – Jardim Roberto, Osasco'),
(06753000, NULL, 'Avenida Intercontinental, 632 – Jardim Caner, Taboão da Serra'),
(06680130, 5, 'Rua N, 32 - Amador Bueno, Itapevi'),
(05033000, 6, 'Rua Guaicurus, 1321 - Água Branca, São Paulo');

insert into tbVendedor (idvendedor, taxafrete) values 
(2, 10),
(4, null),
(5, null),
(6, null),
(7, null);

insert into tbProduto (idvendedor, nome, quantidade, condicao, precounitario, peso, `status`) values
(2, "Livro Harry Potter e a Pedra Filosofal", 1, 1, 25.00, 396, "ativo"),
(4, "Mouse Gamer RGB", 50, 0, 89.90, 150, "ativo"),
(4, "Camiseta Estampada", 100, 0, 39.99, 200, "ativo"),
(4, "Fone de Ouvido Bluetooth", 30, 0, 129.90, 250, "suspenso"),
(4, "Garrafa Térmica 500ml", 40, 0, 59.00, 300, "inativo"),
(5, 'Tênis Nike Air Max 270', NULL, 0, 699.90, 850, 'disponível'),
(5, 'Tênis Adidas Ultraboost', NULL, 0, 749.90, 800, 'disponível'),
(5, 'Sandália Havaianas Slim', NULL, 0, 49.90, 200, 'disponível'),
(5, 'Chinelo Rider Infinity', 20, 0, 79.90, 250, 'ativo'),
(5, 'Tênis Puma Suede Classic', NULL, 0, 399.90, 780, 'disponível'),
(6, 'Monitor Gamer LG 27" 144Hz', NULL, 0, 1899.00, 4500, 'disponível'),
(6, 'Placa de Vídeo NVIDIA RTX 4070', NULL, 0, 5699.00, 1200, 'disponível'),
(6, 'SSD Samsung 1TB NVMe', NULL, 0, 599.90, 60, 'disponível'),
(6, 'Teclado Mecânico Redragon', NULL, 0, 349.90, 950, 'disponível'),
(6, 'Fone de Ouvido Bluetooth JBL', NULL, 0, 299.90, 200, 'disponível'),
(7, 'Smartphone Samsung Galaxy', 10, 0, 2500.00, 0.3, 'ativo'), -- 1 Eletrônicos e Tecnologias
(7, 'iPhone 14', 15, 0, 5000.00, 0.2, 'ativo'), -- 2 Celulares
(7, 'Capinha para Celular', 50, 0, 49.90, 0.1, 'ativo'), -- 3 Acessórios para Celulares
(7, 'Notebook Dell Inspiron', 8, 0, 4500.00, 2.5, 'ativo'), -- 4 Computadores
(7, 'Notebook Asus VivoBook', 12, 0, 4000.00, 2.3, 'ativo'), -- 5 Notebooks
(7, 'iPad Pro', 7, 0, 5500.00, 0.5, 'ativo'), -- 6 Tablets
(7, 'Smart TV 55"', 5, 0, 3000.00, 10, 'ativo'), -- 7 TVs e Áudio
(7, 'Geladeira Frost Free', 3, 0, 2500.00, 50, 'ativo'), -- 8 Eletrodomésticos
(7, 'Camisa Polo Masculina', 30, 0, 120.00, 0.3, 'ativo'), -- 9 Moda e Vestuário
(7, 'Vestido Feminino', 20, 0, 150.00, 0.4, 'ativo'), -- 10 Roupas Femininas
(7, 'Camisa Masculina', 25, 0, 100.00, 0.3, 'ativo'), -- 11 Roupas Masculinas
(7, 'Body Infantil', 40, 0, 80.00, 0.2, 'ativo'), -- 12 Roupas Infantis
(7, 'Tênis Nike Air', 15, 0, 350.00, 0.5, 'ativo'), -- 13 Tênis
(7, 'Chinelo Havaianas', 50, 0, 45.00, 0.2, 'ativo'), -- 14 Sandálias e Chinelos
(7, 'Mochila Escolar', 20, 0, 120.00, 0.7, 'ativo'), -- 15 Bolsas e Mochilas
(7, 'Relógio de Pulso', 30, 0, 250.00, 0.2, 'ativo'), -- 16 Relógios
(7, 'Colar de Prata', 15, 0, 180.00, 0.1, 'ativo'), -- 17 Joias e Bijuterias
(7, 'Sofá 3 Lugares', 3, 0, 2000.00, 30, 'ativo'), -- 18 Casa, Móveis e Jardim
(7, 'Quadro Decorativo', 10, 0, 150.00, 1, 'ativo'), -- 19 Decoração
(7, 'Luminária de Mesa', 12, 0, 120.00, 1.5, 'ativo'), -- 20 Iluminação
(7, 'Rack para Sala', 5, 0, 800.00, 15, 'ativo'), -- 21 Móveis para Sala
(7, 'Cama Casal', 4, 0, 1200.00, 25, 'ativo'), -- 22 Móveis para Quarto
(7, 'Conjunto de Panelas', 10, 0, 350.00, 5, 'ativo'), -- 23 Cozinha e Utensílios
(7, 'Jogo de Cama Queen', 8, 0, 300.00, 3, 'ativo'), -- 24 Cama, Mesa e Banho
(7, 'Kit Jardinagem', 15, 0, 200.00, 2, 'ativo'), -- 25 Jardim e Ferramentas
(7, 'Kit de Maquiagem', 20, 0, 150.00, 0.5, 'ativo'), -- 26 Beleza e Cuidados Pessoais
(7, 'Batom Matte', 50, 0, 35.00, 0.1, 'ativo'), -- 27 Maquiagem
(7, 'Perfume Importado', 25, 0, 300.00, 0.3, 'ativo'), -- 28 Perfumes
(7, 'Shampoo Profissional', 30, 0, 50.00, 0.5, 'ativo'), -- 29 Cabelos
(7, 'Creme Hidratante', 40, 0, 60.00, 0.3, 'ativo'), -- 30 Cuidados com a Pele
(7, 'Kit Barbearia', 20, 0, 150.00, 1, 'ativo'), -- 31 Barbearia e Cuidados Masculinos
(7, 'Cesta de Alimentos', 15, 0, 200.00, 5, 'ativo'), -- 32 Alimentos e Bebidas
(7, 'Frutas Frescas', 50, 0, 100.00, 10, 'ativo'), -- 33 Frutas e Verduras
(7, 'Bandeja de Frios', 25, 0, 180.00, 3, 'ativo'), -- 34 Carnes e Frios
(7, 'Suco Natural', 40, 0, 15.00, 1, 'ativo'), -- 35 Bebidas Não Alcoólicas
(7, 'Vinho Tinto', 20, 0, 120.00, 1.5, 'ativo'), -- 36 Vinhos
(7, 'Cerveja Artesanal', 30, 0, 20.00, 0.5, 'ativo'), -- 37 Cervejas
(7, 'Chocolates Sortidos', 50, 0, 80.00, 2, 'ativo'), -- 38 Snacks e Doces
(7, 'Bola de Futebol', 15, 0, 120.00, 0.6, 'ativo'), -- 39 Esportes e Fitness
(7, 'Bicicleta MTB', 5, 0, 2500.00, 15, 'ativo'), -- 40 Bicicletas
(7, 'Camisa de Time', 20, 0, 150.00, 0.3, 'ativo'), -- 41 Futebol
(7, 'Tênis de Corrida', 25, 0, 300.00, 0.5, 'ativo'), -- 42 Corrida
(7, 'Óculos de Natação', 30, 0, 60.00, 0.2, 'ativo'), -- 43 Natação
(7, 'Halteres Ajustáveis', 10, 0, 400.00, 5, 'ativo'), -- 44 Academia e Musculação
(7, 'Tapete de Yoga', 20, 0, 150.00, 1, 'ativo'), -- 45 Yoga e Pilates
(7, 'Carrinho de Bebê', 8, 0, 1200.00, 12, 'ativo'), -- 46 Bebês, Crianças e Maternidade
(7, 'Carrinho Infantil', 15, 0, 500.00, 5, 'ativo'), -- 47 Carrinhos de Bebê
(7, 'Brinquedo Educativo', 30, 0, 80.00, 0.7, 'ativo'), -- 48 Brinquedos
(7, 'Fraldas Descartáveis', 40, 0, 120.00, 2, 'ativo'), -- 49 Fraldas e Higiene
(7, 'Macacão para Bebê', 20, 0, 100.00, 0.5, 'ativo'), -- 50 Roupas para Bebê
(7, 'Suplementos Vitamínicos', 30, 0, 200.00, 1, 'ativo'), -- 51 Saúde e Bem-Estar
(7, 'Vitaminas em Cápsulas', 40, 0, 150.00, 0.5, 'ativo'), -- 52 Vitaminas e Suplementos
(7, 'Kit Primeiros Socorros', 15, 0, 100.00, 1, 'ativo'), -- 53 Primeiros Socorros
(7, 'Equipamento Médico', 5, 0, 2000.00, 10, 'ativo'), -- 54 Equipamentos Médicos
(7, 'Óleo de Motor', 20, 0, 100.00, 1, 'ativo'), -- 55 Automotivo e Industrial
(7, 'Peça de Carro', 10, 0, 250.00, 2, 'ativo'), -- 56 Peças para Carros
(7, 'Acessório Automotivo', 15, 0, 80.00, 0.5, 'ativo'), -- 57 Acessórios para Carros
(7, 'Capacete de Moto', 8, 0, 300.00, 1, 'ativo'), -- 58 Motos
(7, 'Pneu Aro 17', 12, 0, 400.00, 10, 'ativo'), -- 59 Pneus e Rodas
(7, 'Ração para Cães', 30, 0, 120.00, 5, 'ativo'), -- 60 Animais de Estimação
(7, 'Petisco para Gatos', 40, 0, 50.00, 1, 'ativo'), -- 61 Rações
(7, 'Cama para Cachorro', 15, 0, 200.00, 3, 'ativo'), -- 62 Petiscos
(7, 'Brinquedo para Pet', 30, 0, 80.00, 0.5, 'ativo'), -- 63 Camas e Arranhadores
(7, 'Aquário 30L', 5, 0, 300.00, 5, 'ativo'), -- 64 Brinquedos para Pets
(7, 'Kit Peixes Tropicais', 10, 0, 150.00, 2, 'ativo'), -- 65 Aquarismo
(7, 'Furadeira Elétrica', 10, 0, 400.00, 5, 'ativo'), -- 66 Ferramentas e Construção
(7, 'Parafusadeira', 15, 0, 250.00, 3, 'ativo'), -- 67 Ferramentas Elétricas
(7, 'Martelo Manual', 20, 0, 50.00, 1, 'ativo'), -- 68 Ferramentas Manuais
(7, 'Cimento 50kg', 30, 0, 40.00, 50, 'ativo'), -- 69 Materiais de Construção
(7, 'Tinta Acrílica', 25, 0, 100.00, 15, 'ativo'), -- 70 Tintas
(7, 'Luvas de Segurança', 20, 0, 30.00, 0.5, 'ativo'), -- 71 Segurança do Trabalho
(7, 'Kit de Lazer', 15, 0, 300.00, 5, 'ativo'), -- 72 Lazer, Hobbies e Entretenimento
(7, 'Livro de Romance', 30, 0, 50.00, 0.5, 'ativo'), -- 73 Livros
(7, 'Console PlayStation 5', 5, 0, 4500.00, 5, 'ativo'), -- 74 Videogames
(7, 'Jogo de Tabuleiro', 20, 0, 150.00, 1, 'ativo'), -- 75 Jogos de Tabuleiro
(7, 'Violão Acústico', 10, 0, 600.00, 3, 'ativo'), -- 76 Instrumentos Musicais
(7, 'Kit de Artesanato', 15, 0, 200.00, 2, 'ativo'), -- 77 Artesanato
(7, 'Caderno Universitário', 30, 0, 20.00, 0.5, 'ativo'), -- 78 Papelaria, Escritório e Escola
(7, 'Lápis de Cor', 50, 0, 15.00, 0.2, 'ativo'), -- 79 Materiais Escolares
(7, 'Cartucho de Impressora', 20, 0, 250.00, 0.5, 'ativo'), -- 80 Impressoras e Suprimentos
(7, 'Cadeira de Escritório', 5, 0, 450.00, 15, 'ativo'), -- 81 Móveis de Escritório
(7, 'Mala de Viagem', 15, 0, 300.00, 5, 'ativo'), -- 82 Viagem e Bagagem
(7, 'Mala Executiva', 10, 0, 400.00, 6, 'ativo'), -- 83 Malas
(7, 'Mochila de Viagem', 20, 0, 250.00, 2, 'ativo'), -- 84 Mochilas de Viagem
(7, 'Kit de Acessórios de Viagem', 15, 0, 150.00, 1, 'ativo'), -- 85 Acessórios de Viagem
(7, 'Decoração para Eventos', 10, 0, 200.00, 2, 'ativo'), -- 86 Eventos e Festas
(7, 'Artigos para Casamento', 8, 0, 300.00, 3, 'ativo'), -- 87 Artigos para Casamento
(7, 'Balões e Decoração', 20, 0, 100.00, 1, 'ativo'), -- 88 Decoração de Festas
(7, 'Fantasia Infantil', 15, 0, 150.00, 1, 'ativo'), -- 89 Fantasias
(7, 'Enfeites de Natal', 10, 0, 80.00, 1, 'ativo'); -- 90 Decoração Sazonal

insert into tbImagemProduto (IdProduto, S3Key, IsPrincipal) values 
(16, 's24.png', 1);

insert into tbCategoria_Produto (IdCategoria, IdProduto) values
(73, 1),(72, 1),
(1, 2),(4, 2),(5, 2),
(9, 3),(10, 3),(11, 3),
(1, 4),(7, 4),(3, 4),
(23, 5),(39, 5),(44, 5),(42, 5),(85, 5),
(13, 6),
(13, 7),
(14, 8),
(14, 9),
(13, 10),
(1, 11),(7, 11),
(4, 12),(1, 12),(5, 12),
(4, 13),(1, 13),
(4, 14),(1, 14),
(7, 15),(3, 15),
(1, 16), (2, 16), (3, 16),
(1, 17), (2, 17), (3, 17),
(2, 18), (3, 18),
(4, 19), (5, 19),
(4, 20), (5, 20),
(1, 21), (6, 21),
(1, 22), (7, 22),
(1, 23), (8, 23),
(9, 24), (11, 24),
(9, 25), (10, 25),
(9, 26), (11, 26),
(9, 27), (12, 27),
(9, 28), (13, 28),
(13, 29), (14, 29),
(9, 30), (15, 30),
(16, 31), (17, 31),
(16, 32), (17, 32),
(18, 33), (19, 33),
(18, 34), (19, 34),
(18, 36), (21, 36),
(18, 37), (22, 37),
(18, 38), (23, 38),
(18, 39), (24, 39),
(18, 40), (25, 40),
(26, 41), (27, 41),
(26, 42), (27, 42),
(26, 43), (28, 43),
(26, 44), (29, 44),
(26, 45), (30, 45),
(26, 46), (31, 46),
(32, 47), (33, 47),
(32, 48), (33, 48),
(32, 49), (34, 49),
(32, 50), (35, 50),
(32, 51), (36, 51),
(32, 52), (37, 52),
(32, 53), (38, 53),
(39, 54), (42, 54),
(39, 55), (40, 55),
(39, 56), (41, 56),
(39, 57), (42, 57),
(42, 58), (43, 58),
(44, 59), (45, 59),
(44, 60), (45, 60),
(46, 61), (47, 61),
(46, 62), (47, 62),
(46, 63), (48, 63),
(46, 65), (50, 65),
(51, 66), (52, 66),
(51, 67), (52, 67),
(51, 68), (53, 68),
(51, 69), (54, 69),
(55, 70), (56, 70),
(55, 71), (56, 71),
(55, 72), (57, 72),
(55, 73), (58, 73),
(55, 74), (59, 74),
(60, 75), (61, 75),
(60, 76), (61, 76),
(60, 77), (62, 77),
(60, 78), (63, 78),
(60, 79), (64, 79),
(60, 80), (65, 80),
(66, 81), (67, 81),
(66, 82), (67, 82),
(66, 83), (68, 83),
(66, 84), (69, 84),
(66, 85), (70, 85),
(66, 86), (71, 86),
(72, 87), (73, 87),
(72, 88), (73, 88),
(72, 89), (74, 89),
(72, 90), (75, 90),
(72, 91), (76, 91),
(72, 92), (77, 92),
(78, 93), (79, 93),
(78, 94), (79, 94),
(78, 95), (80, 95),
(78, 96), (81, 96),
(82, 97), (83, 97),
(82, 98), (83, 98),
(82, 99), (84, 99),
(82, 100), (85, 100),
(86, 101), (87, 101),
(86, 102), (87, 102),
(86, 103), (88, 103),
(86, 104), (89, 104),
(86, 105), (90, 105);

insert into tbCartao (IdCliente, NumeroCartao, Bandeira, Modalidade, DataValidade, NomeTitular) values
(1, '4111111111111111', 'Visa', 0, '2027-05-01', 'GILMAR JÚNIOR'),
(2, '5500000000000004', 'Mastercard', 1, '2026-11-01', 'BIANCA GUIMARÃES'),
(2, '5105105105105100', 'Mastercard', 0, '2027-07-01', 'BIANCA GUIMARÃES'),
(2, '4000056655665556', 'Visa', 1, '2028-03-01', 'LÚCIA GUIMARÃES'),
(3, '340000000000009', 'American Express', 0, '2028-08-01', 'LUCAS TOBIAS'),
(4, '6011000000000004', 'Discover', 1, '2025-12-01', 'MÁRCIA CRUZ');

insert into tbNotificacao (mensagem, tabelacontexto) values
("Seu produto foi vendido!", "tbProduto"),
("%p está em promoção!", "tbProduto"),
("Você tem uma nova mensagem!", "tbTicket");

insert into tbCentro_Distribuicao (idendereco, nome) values
(7, "CD Santo Amaro"),
(8, "CD Barueri"),
(9, "CD Santo André"),
(10, "CD Osasco"),
(11, "CD Taboão da Serra"); 

insert into tbEmpresa_Parceira (idempresa, cnpj) values 
(5, 05126536000252),
(6, 05570714000159);

insert into tbLista_Desejo (idcliente, idproduto) values 
(1, 1),
(2, 34),(2, 12),(2, 76),(2, 100),
(3, 10),(3, 29),(3, 53),(3, 54),(3, 55),(3, 101),
(4, 34),(4, 88),(4, 40);

insert into tbCarrinho (idcliente, idproduto, quantidade) values
(1, 1, 1),
(3, 3, 1),
(3, 9, 2);

insert into tbPergunta (idcliente, idproduto, pergunta) values
(1, 1, "De qual ano é a edição desse livro?");

update tbPergunta set resposta = "2018" where idpergunta = 1;

insert into tbPedido (idcomprador, idenderecoentrega) values 
(1, 2),
(3, 5),
(1, 2),
(2, 4),
(3, 5),
(4, 6);

insert into tbProduto_Pedido (idpedido, idproduto, quantidade, tipoentrega, valor, frete) values
(1, 1, 1, 0, 25, 10),
(2, 3, 1, 1, 39.99, 5),(2, 9, 2, 1, 79.90, 5),
(3, 16, 1, 1, 200, 5),(3, 67, 1, 1, 200, 5),(3, 100, 1, 1, 200, 5),(3, 32, 1, 1, 200, 5),
(4, 16, 1, 1, 200, 5),(4, 67, 1, 1, 200, 5),(4, 100, 1, 1, 200, 5),
(5, 16, 1, 1, 200, 5),(5, 67, 1, 1, 200, 5),
(6, 16, 1, 1, 200, 5);

update tbProduto set quantidade = (quantidade - 1), `status` = "inativo" where IdProduto = 1;
update tbProduto set quantidade = (quantidade - 1) where IdProduto = 3;
update tbProduto set quantidade = (quantidade - 2) where IdProduto = 9;

update tbPedido set total = 25, frete = 10 where idpedido = 1;
update tbPedido set total = 119.89, frete = 6 where idpedido = 2;

delete from tbLista_Desejo where idcliente = 1 and idproduto = 1;

delete from tbCarrinho where idcliente = 1;
delete from tbCarrinho where idcliente = 3;

insert into tbPagamento (idpedido, formapagamento, idcartao) values
(1, "credito", 1),
(2, "pix", NULL);

update tbPedido set statuspagamento = "pago" where idpedido = 1;
update tbPedido set statuspagamento = "pago" where idpedido = 2;

insert into tbNotificacao_Cliente (idnotificacao, idcliente, idcontexto) values
(1, 2, 1),
(1, 4, 3),
(1, 5, 9);

insert into tbEntrega(idpedido, remetente, idremetente, destinatario, iddestinatario) values
(1, "vendedor", 2, "comprador", 1),
(2, "vendedor", 4, "cd", 1),
(2, "cd", 1, "cd", 9),
(2, "vendedor", 5, "cd", 8),
(2, "cd", 8, "cd", 9),
(2, "cd", 9, "comprador", 3);

insert into tbEntrega_Produto(identrega, idproduto) values
(1, 1),
(2, 3),
(3, 3),
(4, 9),
(5, 9),
(6, 3),
(6, 9);

insert into tbEntregador(identregador, idendereco, cnh) values
(1, 1, 12345678901);

update tbEntregador set eligibilidade = 1 where identregador = 1;

update tbEntrega set `status` = "entregue" where identrega = 1;
update tbEntrega set `status` = "entregue" where identrega = 2;
update tbEntrega set `status` = "entregue" where identrega = 3;
update tbEntrega set `status` = "entregue" where identrega = 4;
update tbEntrega set `status` = "entregue" where identrega = 5;

insert into tbEntrega_Final (identrega, identregador) values (6, 1);

update tbEntrega set `status` = "entregue" where identrega = 6;

insert into tbReview(idcliente, idproduto, nota, comentario) values
(1, 1, 5, "Amei o livro, veio em ótimas condições!"),
(3, 9, 3, "Produto de má qualidade, mas o que posso esperar por um produto tão barato?");

update tbVendedor set nota = 5 where idvendedor = 2;
update tbVendedor set nota = 3 where idvendedor = 5;

insert into tbPedido_Interno (idproduto, quantidade, idempresa, idfuncionario, idcd) values
(6, 100, 5, 1, 1),(6, 100, 5, 1, 2),(6, 100, 5, 1, 3),(6, 100, 5, 1, 4),(6, 100, 5, 1, 5),
(7, 100, 5, 1, 1),(7, 100, 5, 1, 2),(7, 100, 5, 1, 3),(7, 100, 5, 1, 4),(7, 100, 5, 1, 5),
(8, 50, 5, 1, 1),(8, 50, 5, 1, 2),(8, 50, 5, 1, 3),(8, 50, 5, 1, 4),(8, 50, 5, 1, 5),
(13, 50, 6, 2, 1),(13, 50, 6, 2, 2),(13, 50, 6, 2, 3),(13, 50, 6, 2, 4),
(14, 100, 6, 2, 1),(14, 100, 6, 2, 2),(14, 100, 6, 2, 3),(14, 100, 6, 2, 4);

insert into tbEntrega (idpedidointerno, remetente, idremetente, destinatario, iddestinatario) values
(1, "empresa parceira", 5, "cd", 1),
(2, "empresa parceira", 5, "cd", 2),
(3, "empresa parceira", 5, "cd", 3),
(4, "empresa parceira", 5, "cd", 4),
(5, "empresa parceira", 5, "cd", 5),

(6, "empresa parceira", 5, "cd", 1),
(7, "empresa parceira", 5, "cd", 2),
(8, "empresa parceira", 5, "cd", 3),
(9, "empresa parceira", 5, "cd", 4),
(10, "empresa parceira", 5, "cd", 5),

(11, "empresa parceira", 5, "cd", 1),
(12, "empresa parceira", 5, "cd", 2),
(13, "empresa parceira", 5, "cd", 3),
(14, "empresa parceira", 5, "cd", 4),
(15, "empresa parceira", 5, "cd", 5),

(16, "empresa parceira", 6, "cd", 1),
(17, "empresa parceira", 6, "cd", 2),
(18, "empresa parceira", 6, "cd", 3),
(19, "empresa parceira", 6, "cd", 4),

(20, "empresa parceira", 6, "cd", 1),
(21, "empresa parceira", 6, "cd", 2),
(22, "empresa parceira", 6, "cd", 3),
(23, "empresa parceira", 6, "cd", 4);

update tbEntrega set `status` = "entregue" where identrega between 7 and 29;

insert into tbProduto_Cd (idcd, idproduto, quantidade) values
(1, 6, 100), (2, 6, 100), (3, 6, 100), (4, 6, 100), (5, 6, 100),
(1, 7, 100), (2, 7, 100), (3, 7, 100), (4, 7, 100), (5, 7, 100),
(1, 8, 50), (2, 8, 50), (3, 8, 50), (4, 8, 50), (5, 8, 50),
(1, 13, 50), (2, 13, 50), (3, 13, 50), (4, 13, 50),
(1, 14, 100), (2, 14, 100), (3, 14, 100), (4, 14, 100);

insert into tbLista_Desejo (idcliente, idproduto) values (4, 13);

insert into tbLista_Desejo (idcliente, idproduto) values (9, 13);



insert into tbPromocao (idproduto, desconto, validaate) values 
(1, 20, null),
(2, 15, null),
(3, 20, null),
(4, 12, null),
(5, 20, null),
(6, 10, null),
(7, 20, null),
(8, 20, null),
(9, 25, null),
(10, 20, null);

insert into tbNotificacao_Cliente (idnotificacao, idcliente, idcontexto) values (2, 4, 13);

insert into tbTicket (idcliente) values (3);

insert into tbMensagem (idticket, mensagem, remetente, idremetente) values
(1, "Olá, fiz um pedido de uma camiseta estampada há quase um mês e ela não chegou no meu endereço, 
mas no site da Selecta diz que foi entregue. Alguém pode me ajudar?", "cliente", 3);

update tbTicket set `status` = "ativo", idfuncionario = 1 where idticket = 1;

insert into tbMensagem (idticket, mensagem, remetente, idremetente) values
(1, "Olá Lucas, pedimos desculpa pela inconveniência, estamos investigando o que aconteceu com o seu produto.
Tentaremos garantir que receberá o produto dentro de uma semana.", "funcionario", 1);

insert into tbNotificacao_Cliente (idnotificacao, idcliente, idcontexto) values
(3, 3, 1);

update tbTicket set idfuncionario = 2 where idticket = 1;

insert into tbMensagem (idticket, mensagem, remetente, idremetente) values
(1, "Olá Lucas, poderia nos dar mais detalhes sobre o ocorrido?", "funcionario", 2);

insert into tbNotificacao_Cliente (idnotificacao, idcliente, idcontexto) values
(3, 3, 1);

insert into tbMensagem (idticket, mensagem, remetente, idremetente) values
(1, "Meu produto chegou hoje. Obrigado pela atenção no atendimento.", "cliente", 3);

update tbTicket set `status` = "arquivado" where idticket = 1;

 select * from tbCategoria;
 select * from tbProduto;
-- select * from tbProduto_Cd;
select * from tbCliente;
-- select * from tbCentro_Distribuicao;
 select * from tbVendedor;
select * from tbEntregador;
-- select * from tbEntrega;
-- select * from tbEntrega_Produto;
-- select * from tbEntrega_Final;
 select * from tbImagemProduto;	
 select * from tbPedido;
-- select * from tbPedido_Interno;
 select * from tbProduto_Pedido;
-- select * from tbMensagem;
-- select * from tbReview;
 select * from tbFuncionario;
-- select * from tbPagamento;
-- select * from tbCartao;
-- select * from tbHistorico;
-- select * from tbEndereco;
-- select * from tbEmpresa_Parceira;
 select * from tbPromocao;
 select * from tbCarrinho;
 select * from tbLista_Desejo;
-- select * from tbTicket;
-- select * from tbNotificacao;
	select * from tbCategoria_Produto;
 select * from tbNotificacao_Cliente;
 
 select * from tbProduto where Idproduto = 80;
 
 -- ========================================
-- AJUSTE DE FOREIGN KEYS - ESTRUTURA ATUAL
-- ========================================

-- tbCategoria_Cliente
alter table tbCategoria_Cliente
drop foreign key tbCategoria_Cliente_ibfk_1,
drop foreign key tbCategoria_Cliente_ibfk_2;

alter table tbCategoria_Cliente
add constraint fk_categoria_cliente_categoria
foreign key (IdCategoria) references tbCategoria(IdCategoria)
on delete cascade on update cascade,
add constraint fk_categoria_cliente_cliente
foreign key (IdCliente) references tbCliente(IdCliente)
on delete cascade on update cascade;

-- tbCartao
alter table tbCartao
drop foreign key tbcartao_ibfk_1;

alter table tbCartao
add constraint fk_cartao_cliente
foreign key (IdCliente) references tbCliente(IdCliente)
on delete cascade on update cascade;

-- tbEndereco
alter table tbEndereco
drop foreign key tbendereco_ibfk_1;

alter table tbEndereco
add constraint fk_endereco_cliente
foreign key (IdCliente) references tbCliente(IdCliente)
on delete set null on update cascade;

-- tbProduto
alter table tbProduto
drop foreign key tbproduto_ibfk_1;

alter table tbProduto
add constraint fk_produto_vendedor
foreign key (IdVendedor) references tbCliente(IdCliente)
on delete cascade on update cascade;

-- tbCategoria_Produto
alter table tbCategoria_Produto
drop foreign key tbCategoria_Produto_ibfk_1,
drop foreign key tbCategoria_Produto_ibfk_2;

alter table tbCategoria_Produto
add constraint fk_categoria_produto_categoria
foreign key (IdCategoria) references tbCategoria(IdCategoria)
on delete cascade on update cascade,
add constraint fk_categoria_produto_produto
foreign key (IdProduto) references tbProduto(IdProduto)
on delete cascade on update cascade;

-- tbCentro_Distribuicao
alter table tbCentro_Distribuicao
drop foreign key tbcentro_distribuicao_ibfk_1;

alter table tbCentro_Distribuicao
add constraint fk_cd_endereco
foreign key (IdEndereco) references tbEndereco(IdEndereco)
on delete cascade on update cascade;

-- tbEmpresa_Parceira
alter table tbEmpresa_Parceira
drop foreign key tbempresa_parceira_ibfk_1;

alter table tbEmpresa_Parceira
add constraint fk_empresa_cliente
foreign key (IdEmpresa) references tbCliente(IdCliente)
on delete cascade on update cascade;

-- tbPromocao
alter table tbPromocao
drop foreign key tbpromocao_ibfk_1;

alter table tbPromocao
add constraint fk_promocao_produto
foreign key (IdProduto) references tbProduto(IdProduto)
on delete cascade on update cascade;

-- tbNotificacao_Cliente
alter table tbNotificacao_Cliente
drop foreign key tbnotificacao_cliente_ibfk_1,
drop foreign key tbnotificacao_cliente_ibfk_2;

alter table tbNotificacao_Cliente
add constraint fk_notificacao_cliente_notificacao
foreign key (IdNotificacao) references tbNotificacao(IdNotificacao)
on delete cascade on update cascade,
add constraint fk_notificacao_cliente_cliente
foreign key (IdCliente) references tbCliente(IdCliente)
on delete cascade on update cascade;

-- tbLista_Desejo
alter table tbLista_Desejo
drop foreign key tblista_desejo_ibfk_1,
drop foreign key tblista_desejo_ibfk_2;

alter table tbLista_Desejo
add constraint fk_lista_desejo_produto
foreign key (IdProduto) references tbProduto(IdProduto)
on delete cascade on update cascade,
add constraint fk_lista_desejo_cliente
foreign key (IdCliente) references tbCliente(IdCliente)
on delete cascade on update cascade;

-- tbCarrinho
alter table tbCarrinho
drop foreign key tbcarrinho_ibfk_1,
drop foreign key tbcarrinho_ibfk_2;

alter table tbCarrinho
add constraint fk_carrinho_produto
foreign key (IdProduto) references tbProduto(IdProduto)
on delete cascade on update cascade,
add constraint fk_carrinho_cliente
foreign key (IdCliente) references tbCliente(IdCliente)
on delete cascade on update cascade;

-- tbReview
alter table tbReview
drop foreign key tbreview_ibfk_1,
drop foreign key tbreview_ibfk_2;

alter table tbReview
add constraint fk_review_cliente
foreign key (IdCliente) references tbCliente(IdCliente)
on delete cascade on update cascade,
add constraint fk_review_produto
foreign key (IdProduto) references tbProduto(IdProduto)
on delete cascade on update cascade;

-- tbTicket
alter table tbTicket
drop foreign key tbticket_ibfk_1,
drop foreign key tbticket_ibfk_2;

alter table tbTicket
add constraint fk_ticket_cliente
foreign key (IdCliente) references tbCliente(IdCliente)
on delete cascade on update cascade,
add constraint fk_ticket_funcionario
foreign key (IdFuncionario) references tbFuncionario(IdFuncionario)
on delete set null on update cascade;

-- tbMensagem
alter table tbMensagem
drop foreign key tbmensagem_ibfk_1;

alter table tbMensagem
add constraint fk_mensagem_ticket
foreign key (IdTicket) references tbTicket(IdTicket)
on delete cascade on update cascade;

-- tbPedido
alter table tbPedido
drop foreign key tbpedido_ibfk_1,
drop foreign key tbpedido_ibfk_2;

alter table tbPedido
add constraint fk_pedido_endereco
foreign key (IdEnderecoEntrega) references tbEndereco(IdEndereco)
on delete restrict on update cascade,
add constraint fk_pedido_cliente
foreign key (IdComprador) references tbCliente(IdCliente)
on delete cascade on update cascade;

-- tbPagamento
alter table tbPagamento
drop foreign key tbpagamento_ibfk_1,
drop foreign key tbpagamento_ibfk_2;

alter table tbPagamento
add constraint fk_pagamento_pedido
foreign key (IdPedido) references tbPedido(IdPedido)
on delete cascade on update cascade,
add constraint fk_pagamento_cartao
foreign key (IdCartao) references tbCartao(IdCartao)
on delete set null on update cascade;

-- tbProduto_Pedido
alter table tbProduto_Pedido
drop foreign key tbproduto_pedido_ibfk_1,
drop foreign key tbproduto_pedido_ibfk_2;

alter table tbProduto_Pedido
add constraint fk_produto_pedido_produto
foreign key (IdProduto) references tbProduto(IdProduto)
on delete cascade on update cascade,
add constraint fk_produto_pedido_pedido
foreign key (IdPedido) references tbPedido(IdPedido)
on delete cascade on update cascade;

-- tbPedido_Interno
alter table tbPedido_Interno
drop foreign key tbpedido_interno_ibfk_1,
drop foreign key tbpedido_interno_ibfk_2,
drop foreign key tbpedido_interno_ibfk_3,
drop foreign key tbpedido_interno_ibfk_4;

alter table tbPedido_Interno
add constraint fk_pedido_interno_produto
foreign key (IdProduto) references tbProduto(IdProduto)
on delete cascade on update cascade,
add constraint fk_pedido_interno_empresa
foreign key (IdEmpresa) references tbEmpresa_Parceira(IdEmpresa)
on delete cascade on update cascade,
add constraint fk_pedido_interno_funcionario
foreign key (IdFuncionario) references tbFuncionario(IdFuncionario)
on delete cascade on update cascade,
add constraint fk_pedido_interno_cd
foreign key (IdCD) references tbCentro_Distribuicao(IdCD)
on delete cascade on update cascade;

-- tbProduto_Cd
alter table tbProduto_Cd
drop foreign key tbproduto_cd_ibfk_1,
drop foreign key tbproduto_cd_ibfk_2;

alter table tbProduto_Cd
add constraint fk_produto_cd_cd
foreign key (IdCD) references tbCentro_Distribuicao(IdCD)
on delete cascade on update cascade,
add constraint fk_produto_cd_produto
foreign key (IdProduto) references tbProduto(IdProduto)
on delete cascade on update cascade;

-- tbEntrega
alter table tbEntrega
drop foreign key tbentrega_ibfk_1,
drop foreign key tbentrega_ibfk_2;

alter table tbEntrega
add constraint fk_entrega_pedido
foreign key (IdPedido) references tbPedido(IdPedido)
on delete set null on update cascade,
add constraint fk_entrega_pedido_interno
foreign key (IdPedidoInterno) references tbPedido_Interno(IdPedidoInterno)
on delete set null on update cascade;

-- tbEntrega_Produto
alter table tbEntrega_Produto
drop foreign key tbentrega_produto_ibfk_1,
drop foreign key tbentrega_produto_ibfk_2;

alter table tbEntrega_Produto
add constraint fk_entrega_produto_entrega
foreign key (IdEntrega) references tbEntrega(IdEntrega)
on delete cascade on update cascade,
add constraint fk_entrega_produto_produto
foreign key (IdProduto) references tbProduto(IdProduto)
on delete cascade on update cascade;

-- tbEntregador
alter table tbEntregador
drop foreign key tbentregador_ibfk_1,
drop foreign key tbentregador_ibfk_2;

alter table tbEntregador
add constraint fk_entregador_endereco
foreign key (IdEndereco) references tbEndereco(IdEndereco)
on delete cascade on update cascade,
add constraint fk_entregador_cliente
foreign key (IdEntregador) references tbCliente(IdCliente)
on delete cascade on update cascade;

-- tbVendedor
alter table tbVendedor
drop foreign key tbvendedor_ibfk_1;

alter table tbVendedor
add constraint fk_vendedor_cliente
foreign key (IdVendedor) references tbCliente(IdCliente)
on delete cascade on update cascade;

-- tbEntrega_Final
alter table tbEntrega_Final
drop foreign key tbentrega_final_ibfk_1,
drop foreign key tbentrega_final_ibfk_2;

alter table tbEntrega_Final
add constraint fk_entrega_final_entrega
foreign key (IdEntrega) references tbEntrega(IdEntrega)
on delete cascade on update cascade,
add constraint fk_entrega_final_entregador
foreign key (IdEntregador) references tbEntregador(IdEntregador)
on delete cascade on update cascade;

-- tbImagemProduto
alter table tbImagemProduto
drop foreign key tbimagemproduto_ibfk_1;

alter table tbImagemProduto
add constraint fk_imagem_produto
foreign key (IdProduto) references tbProduto(IdProduto)
on delete cascade on update cascade;

-- tbPergunta
alter table tbPergunta
drop foreign key tbpergunta_ibfk_1,
drop foreign key tbpergunta_ibfk_2;

alter table tbPergunta
add constraint fk_pergunta_cliente
foreign key (IdCliente) references tbCliente(IdCliente)
on delete cascade on update cascade,
add constraint fk_pergunta_produto
foreign key (IdProduto) references tbProduto(IdProduto)
on delete cascade on update cascade;
