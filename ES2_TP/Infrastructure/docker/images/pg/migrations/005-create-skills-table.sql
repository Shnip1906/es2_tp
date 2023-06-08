create table skills(
                       id_skills uuid primary key default uuid_generate_v4(),
                       nome_skills varchar(100) not null,
                       id_area_profissional uuid,
                       FOREIGN KEY (id_area_profissional) REFERENCES areaProfissional (id_area_profissional)
);