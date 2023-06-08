create table utilizador(
    id_utilizador uuid constraint utilizador_pk primary key default uuid_generate_v4(),
    nome_utilizador varchar(100) not null,
    tipo_utilizador uuid constraint tipo_utilizador_id_fk references tipoUtilizador on
        delete cascade
)