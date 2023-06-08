create table utilizador(
                           id_utilizador uuid primary key default uuid_generate_v4(),
                           nome_utilizador varchar(100) not null,
                           id_tipo_utilizador uuid,
                           FOREIGN KEY (id_tipo_utilizador) REFERENCES tipoUtilizador (id_tipo_utilizador)
);