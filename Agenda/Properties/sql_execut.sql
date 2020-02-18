USE mysql;
DROP DATABASE IF EXISTS agenda;
CREATE DATABASE agenda;
USE agenda;

DROP TABLE IF EXISTS atividades;

CREATE TABLE IF NOT EXISTS atividades(
	id int AUTO_INCREMENT,
	nome varchar(150) NOT NULL,
	descricao varchar(255) NOT NULL,
	datainicio datetime,
	datafim datetime,
PRIMARY KEY (id)
);

INSERT INTO atividades(nome, descricao, datainicio, datafim) 
VALUES 
	("Concluida", "Atividade Comcluida." ,NOW() ,NOW()),
	("Pendente", "É a atividade com a data e hora maior que a data e hora atual." ,CURDATE() + INTERVAL 1 DAY ,CURDATE() + INTERVAL 2 DAY) ;

INSERT INTO atividades(nome, descricao) VALUES ("Não Fazer", "Marcar atividade desnecessária. Obs: Não tem data de inicio nem fim.") ;

INSERT INTO atividades(nome, descricao, datainicio) VALUES ("Concluída", "É a atividade com a data e hora maior que a data e hora atual." ,CURDATE() + INTERVAL 1 DAY) ;

SELECT * FROM atividades;