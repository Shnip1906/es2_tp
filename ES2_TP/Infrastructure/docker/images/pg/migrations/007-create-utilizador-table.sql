create table utilizador(
   id_utilizador uuid primary key default uuid_generate_v4(),
   username varchar(100) not null,
   password varchar(100) not null,
   nome_utilizador varchar(100) not null,
   tipo_utilizador int not null
);