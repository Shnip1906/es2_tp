create table skillProf(
                          id_skillsProf uuid primary key default uuid_generate_v4(),
                          nHoras int not null,
                          id_perfil uuid,
                          id_skills uuid,
                          FOREIGN KEY (id_perfil) REFERENCES perfil (id_perfil),
                          FOREIGN KEY (id_skills) REFERENCES skills (id_skills)
)