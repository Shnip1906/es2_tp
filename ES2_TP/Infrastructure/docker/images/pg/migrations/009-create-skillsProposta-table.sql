create table skillsProposta(
                               id_skills_proposta uuid primary key default uuid_generate_v4(),
                               n_minimo_horas_skill int not null,
                               id_skills uuid,
                               id_propostas uuid,
                               FOREIGN KEY (id_skills) REFERENCES skills (id_skills),
                               FOREIGN KEY (id_propostas) REFERENCES propostas (id_propostas)
)