using System;
using System.Collections.Generic;
using System.Linq;

namespace PROJETO_PETSHOP
{
    /// <summary>
    /// Sample program to demonstrate how
    /// entities and classes from my
    /// academic work should behave.
    /// 
    /// Link to the full project:
    /// https://github.com/marcelothebuilder/UNOPAR-pet-shop
    /// 
    /// Author:
    /// https://github.com/marcelothebuilder/
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Servico servico = new Servico();
            servico.Nome = "Tosa";
            servico.Descricao = "Consiste em um tratamento com direito a uma tosa higiênica e hidratação.";
            servico.Valor = 40.0;

            TipoAnimal tipo = new TipoAnimal();
            tipo.Descricao = "Cachorro";

            Raca raca = new Raca();
            raca.Descricao = "Poodle";
            raca.TipoAnimal = tipo;

            Animal pet = new Animal();
            pet.Nome = "Maycou Douglas";
            pet.Idade = 4;
            pet.Raca = raca;
            pet.Sexo = Sexo.MACHO;

            Agendamento agendamentoServico = new Agendamento();
            agendamentoServico.HoraAgendamento = DateTime.Now;
            agendamentoServico.Servico = servico;
            agendamentoServico.Animal = pet;

            Agenda agenda = new Agenda();

            try {
                Console.WriteLine("Tentando agendar...");
                agenda.adicionaAgendamento(agendamentoServico);
                Console.WriteLine("Agendamento adicionado.");
                Console.WriteLine("Tentando agendar novamente o mesmo lançamento...");
                agenda.adicionaAgendamento(agendamentoServico);
                Console.WriteLine("Agendamento adicionado novamente.");
            } catch(HorarioAgendamentoException e) {
                Console.WriteLine("Não foi possível agendar: " + e.Message);
            }
           

            Agendamento primeiro = agenda.getPrimeiro();

            Console.Write(
                string.Format(
                    "O primeiro:\n"+
                    "*\tEstá marcado para {0}\n"+
                    "*\tdo pet {1} de raça {2}\n"+
                    "*\tpara execução do serviço de {3}\n"+
                    "*\tno valor de R$ {4:#.00}.\n", 
                    primeiro.HoraAgendamento.ToString(), 
                    primeiro.Animal.Nome, 
                    primeiro.Animal.Raca.Descricao,
                    primeiro.Servico.Nome,
                    primeiro.Servico.Valor
                )
            );
        }
                
        public class Agenda {
            public Agenda() {}

            public int NumeroDeAgendamentos {
                get {
                    return listaAgendamentos.Count;
                }
            }

            private List<Agendamento> listaAgendamentos = new List<Agendamento>();

            public void adicionaAgendamento(Agendamento agendamentoServico) {
                int insertionPos = 0;
                // encontra a melhor posição de inserção, onde elementoInserido.horario < prox.horario
                for(insertionPos = 0; insertionPos < listaAgendamentos.Count; insertionPos++) {
                    Agendamento currentPosAgendamento = listaAgendamentos.ElementAt(insertionPos);

                    // não podemos ter agendamento para o mesmo horário
                    // TODO: Devemos checar por uma faixa de horário, considerando o tempo de serviço.
                    if (agendamentoServico.HoraAgendamento == currentPosAgendamento.HoraAgendamento) {
                        throw new HorarioAgendamentoException("Já existe um Agendamento para o mesmo horário.");
                    }

                    // se encontramos uma posiçao que o próximo agendamento é
                    // "mais tarde" que o atual, é aqui que devemos inserir.
                    if (agendamentoServico.HoraAgendamento < currentPosAgendamento.HoraAgendamento) {
                        break;
                    }
                }
                listaAgendamentos.Insert(insertionPos, agendamentoServico);
            }

            public Agendamento getPrimeiro() {
                return listaAgendamentos.FirstOrDefault();
            }

            public Agendamento get(int index) {
                return listaAgendamentos.ElementAt(index);
            }
        }

        /*
            Entities
        */


        public enum Sexo
        {
            MACHO, FEMEA
        }

        public class Agendamento {
            public Animal Animal { get; internal set; }
            public DateTime HoraAgendamento { get; internal set; }
            public Servico Servico { get; internal set; }
        }

        public class Animal {
            public int Idade { get; internal set; }
            public string Nome { get; internal set; }
            public Raca Raca { get; internal set; }
            public object Sexo { get; internal set; }
        }

        public class Raca {
            public string Descricao { get; internal set; }
            public TipoAnimal TipoAnimal { get; internal set; }
        }

        public class Servico {
            public string Descricao { get; internal set; }
            public string Nome { get; internal set; }
            public double Valor { get; internal set; }
        }

        public class TipoAnimal {
            public string Descricao { get; internal set; }

            internal void adicionaRaca(Raca raca) {
                throw new NotImplementedException();
            }
        }

        public class Usuario {
            public string Login { get; internal set; }
            public string Nome { get; internal set; }
            public string Senha { get; internal set; }
        }
    }

    [Serializable]
    internal class HorarioAgendamentoException : Exception {
        public HorarioAgendamentoException(string message) : base(message) {}
    }
}
