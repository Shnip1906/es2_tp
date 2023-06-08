create table skillsProposta(
    id_skills_proposta uuid constraint skill_proposta_pk primary key default uuid_generate_v4(),
    n_minimo_horas_skill int not null,
    id_skill uuid constraint skills_id_fk references skills on
        delete cascade,
    id_proposta uuid constraint proposta_id_fk references proposta on
        delete cascade
)