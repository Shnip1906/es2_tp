create table tipoUtilizador(
    id_tipo_utilizador uuid primary key default uuid_generate_v4(),
    tipo_utilizador varchar(100) not null
)