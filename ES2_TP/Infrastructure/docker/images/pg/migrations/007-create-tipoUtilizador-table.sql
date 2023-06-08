create table tipoUtilizdor(
    id_tipo_utilizador uuid constraint tipo_utilizador_pk primary key default uuid_generate_v4(),
    tipo_utilizador varchar(100) not null,
)