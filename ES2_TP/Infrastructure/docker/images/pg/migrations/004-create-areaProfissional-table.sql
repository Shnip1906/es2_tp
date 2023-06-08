create table areaProfissional(
    id_area_profissional uuid constraint areaProfissioanl_pk primary key default uuid_generate_v4(),
    nome_area_prfossional varchar(100) not null
)