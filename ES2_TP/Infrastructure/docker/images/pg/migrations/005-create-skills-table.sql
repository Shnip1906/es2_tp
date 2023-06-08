create table skills(
    id_skills uuid constraint skill_pk primary key default uuid_generate_v4(),
    nome_skills varchar(100) not null,
    id_area_profissional uuid constraint area_profissional_id_fk references areaProfissional on
        delete cascade
)