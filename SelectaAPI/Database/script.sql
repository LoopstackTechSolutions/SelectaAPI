SET FOREIGN_KEY_CHECKS=0;

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

create table if not exists tbCartao
(
IdCartao int primary key auto_increment,
IdCliente int not null,
NumeroCartao char(16) not null,
Bandeira varchar(30) not null,
Modalidade bit not null,
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
isPrincipal bit default 1,
foreign key (IdCliente) references tbCliente (IdCliente)
);

create table if not exists tbProduto
(
IdProduto int primary key auto_increment,
IdVendedor int not null,
Nome varchar(50) not null,
Quantidade int,
Condicao bit not null,  -- 0 = "novo" / 1 = "usado"
PrecoUnitario decimal(10,2) not null,
Peso int not null, -- em gramas
`Status` varchar(40), -- "ativo"/"inativo"/"suspenso"/"disponível"
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
Logo blob,
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
IsLida bit default 0,
foreign key (IdNotificacao) references tbNotificacao(IdNotificacao),
foreign key (IdCliente) references tbCliente(IdCliente)
);

create table if not exists tbLista_Desejo
(
IdProduto int primary key,
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
TipoEntrega bit not null, -- 0 = entrega própria / 1 = entrega selecta
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
Eligibilidade bit default 0,
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
Imagem blob,
IsPrincipal bit not null, -- 0 = não é principal / 1 = é principal
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
("Kabum", "kabum@kabum.org.br", "12345678");

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
(6, null);

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
(6, 'Fone de Ouvido Bluetooth JBL', NULL, 0, 299.90, 200, 'disponível');

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
(7, 15),(3, 15);

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
(1, 1);

insert into tbCarrinho (idcliente, idproduto, quantidade) values
(1, 1, 1),
(3, 3, 1),
(3, 9, 2);

insert into tbPergunta (idcliente, idproduto, pergunta) values
(1, 1, "De qual ano é a edição desse livro?");

update tbPergunta set resposta = "2018" where idpergunta = 1;

insert into tbPedido (idcomprador, idenderecoentrega) values 
(1, 2),
(3, 5);

insert into tbProduto_Pedido (idpedido, idproduto, quantidade, tipoentrega, valor, frete) values
(1, 1, 1, 0, 25, 10),
(2, 3, 1, 1, 39.99, 5),
(2, 9, 2, 1, 79.90, 5);

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

insert into tbPromocao (idproduto, desconto, validaate) values (13, 20, null);

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

-- select * from tbCategoria;
-- select * from tbProduto;
-- select * from tbProduto_Cd;
-- select * from tbCliente;
-- select * from tbCentro_Distribuicao;
-- select * from tbVendedor;
-- select * from tbEntregador;
-- select * from tbEntrega;
-- select * from tbEntrega_Produto;
-- select * from tbEntrega_Final;
-- select * from tbImagemProduto;
-- select * from tbPedido;
-- select * from tbPedido_Interno;
-- select * from tbProduto_Pedido;
-- select * from tbMensagem;
-- select * from tbReview;
-- select * from tbFuncionario;
-- select * from tbPagamento;
-- select * from tbCartao;
-- select * from tbHistorico;
-- select * from tbEndereco;
-- select * from tbEmpresa_Parceira;
-- select * from tbPromocao;
-- select * from tbCarrinho;
-- select * from tbLista_Desejo;
-- select * from tbTicket;
-- select * from tbNotificacao;
-- select * from tbNotificacao_Cliente;

SET FOREIGN_KEY_CHECKS=1;