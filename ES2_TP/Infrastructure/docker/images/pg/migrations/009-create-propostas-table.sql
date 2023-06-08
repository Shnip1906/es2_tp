create table propostas(
    id_propostas uuid constraint propostas_pk primary key default uuid_generate_v4(),
    nome_propostas varchar(100) not null,
    nTotalHoras int not null,
    descricao varchar(1000) not null,
    id_area_profissional uuid constraint area_profissional_id_fk references areaProfissional on
        delete cascade
)