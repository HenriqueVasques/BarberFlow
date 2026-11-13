using BarberFlow.API.Data.Context;
using BarberFlow.API.Enums;
using BarberFlow.API.Models;
using Bogus;

public static class DbSeeder
{
    public static void SeedData(AppDbContext context, int quantidadeAgendamentos = 10000)
    {
        // 1. GARANTE A SEMENTE FIXA PARA REPRODUTIBILIDADE CIENTÍFICA
        Randomizer.Seed = new Random(42);

        // Se já houver agendamentos com esse volume, evita duplicar desnecessariamente
        if (context.Agendamentos.Any()) return;

        // 2. CRIAR EMPRESA (1 registro)
        var empresa = new Empresa
        {
            Nome = "BarberFlow Matriz",
            Slug = "barberflow-matriz",
            CNPJ = "12345678000199",
            Ativo = true,
            IsDeleted = false,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };
        context.Empresas.Add(empresa);
        context.SaveChanges(); // Salva para gerar o Id da Empresa

        // 3. CRIAR SERVIÇOS (Os 7 serviços fixos)
        var nomesServicos = new[]
        {
            "Corte Clássico / Tesoura",
            "Corte Infantil (Kids)",
            "Barba Tradicional",
            "Combo Corte + Barba",
            "Acabamento / 'Pézinho'",
            "Design de Sobrancelha",
            "Pigmentação (Barba ou Cabelo)"
        };

        var servicos = nomesServicos.Select(nome => new Servico
        {
            EmpresaId = empresa.Id,
            Nome = nome,
            DuracaoMinutos = 30,
            PrecoBase = new Random().Next(30, 100),
            Ativo = true,
            IsDeleted = false,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        }).ToList();
        context.Servicos.AddRange(servicos);
        context.SaveChanges();

        // 4. CRIAR CLIENTES E SEUS RESPECTIVOS USUÁRIOS (300 clientes fixos)
        var clienteFaker = new Faker<Cliente>("pt_BR")
            .RuleFor(c => c.EmpresaId, f => empresa.Id)
            .RuleFor(c => c.Ativo, _ => true)
            .RuleFor(c => c.IsDeleted, _ => false)
            .RuleFor(c => c.AceitouTermosPrivacidade, _ => true)
            .RuleFor(c => c.DataConsentimento, _ => DateTime.UtcNow)
            .RuleFor(c => c.Usuario, f => new Usuario
            {
                EmpresaId = empresa.Id,
                Nome = f.Name.FullName(),
                Email = f.Internet.Email(),
                SenhaHash = "AQAAAAEAACcQAAAAEP...", // Hash genérico válido para teste
                Telefone = f.Phone.PhoneNumber("(##) 9####-####"),
                Perfil = PerfilUsuario.Cliente,
                Ativo = true,
                IsDeleted = false,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            });

        var clientes = clienteFaker.Generate(300);
        context.Clientes.AddRange(clientes);
        context.SaveChanges();

        // 5. CRIAR PROFISSIONAIS E SEUS RESPECTIVOS USUÁRIOS (5 profissionais fixos)
        var profissionalFaker = new Faker<Profissional>("pt_BR")
            .RuleFor(p => p.EmpresaId, f => empresa.Id)
            .RuleFor(p => p.PercentualComissao, _ => 40.0m)
            .RuleFor(p => p.Ativo, _ => true)
            .RuleFor(p => p.IsDeleted, _ => false)
            .RuleFor(p => p.Usuario, f => new Usuario
            {
                EmpresaId = empresa.Id,
                Nome = f.Name.FirstName(),
                Email = f.Internet.Email(),
                SenhaHash = "AQAAAAEAACcQAAAAEP...",
                Telefone = f.Phone.PhoneNumber("(##) 9####-####"),
                Perfil = PerfilUsuario.Profissional,
                Ativo = true,
                IsDeleted = false,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            });

        var profissionais = profissionalFaker.Generate(5);
        context.Profissionais.AddRange(profissionais);
        context.SaveChanges(); // Salva para gerar os IDs dos profissionais no banco

        // 6. CRIAR RELAÇÃO PROFISSIONAL-SERVIÇO (Tabela Associativa)
        var profServicos = new List<ProfissionalServico>();
        foreach (var prof in profissionais)
        {
            // Cada profissional atende aleatoriamente entre 4 a 7 serviços dos 7 disponíveis
            var servicosDoProf = servicos.OrderBy(x => Guid.NewGuid()).Take(new Random().Next(4, 8));
            foreach (var serv in servicosDoProf)
            {
                profServicos.Add(new ProfissionalServico
                {
                    ProfissionalId = prof.Id,
                    ServicoId = serv.Id,
                    Ativo = true,
                    IsDeleted = false,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                });
            }
        }
        context.ProfissionalServicos.AddRange(profServicos);
        context.SaveChanges();

        // 7. CRIAR AGENDAMENTOS (O volume que escala: 10k, 50k, 100k, 250k)
        int tamanhoLote = 5000;
        int lotesCriados = 0;

        while (lotesCriados < quantidadeAgendamentos)
        {
            int qtdParaEsteLote = Math.Min(tamanhoLote, quantidadeAgendamentos - lotesCriados);
            var agendamentoFaker = new Faker<Agendamento>("pt_BR")
                .RuleFor(a => a.EmpresaId, f => empresa.Id)
                .RuleFor(a => a.ClienteId, f => f.PickRandom(clientes).Id)
                .RuleFor(a => a.ProfissionalServicoId, f => f.PickRandom(profServicos).Id)
                // Horário de início em um horizonte de 6 meses
                .RuleFor(a => a.DataHoraInicio, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
                // Horário de fim somando de 30 a 60 minutos
                .RuleFor(a => a.DataHoraFim, (f, a) => a.DataHoraInicio.AddMinutes(f.Random.ArrayElement(new int[] { 30, 45, 60 })))
                // Ponderação realista de status
                .RuleFor(a => a.Status, f => {
                    int r = f.Random.Number(1, 100);
                    if (r <= 50) return StatusAgendamento.Finalizado;
                    if (r <= 80) return StatusAgendamento.Confirmado;
                    if (r <= 90) return StatusAgendamento.Pendente;
                    return StatusAgendamento.Cancelado;
                })
                .RuleFor(a => a.PrecoNoMomento, _ => 50.0m)
                .RuleFor(a => a.DataCriacao, _ => DateTime.UtcNow)
                .RuleFor(a => a.DataAtualizacao, _ => DateTime.UtcNow);

            var loteAgendamentos = agendamentoFaker.Generate(qtdParaEsteLote);
            context.Agendamentos.AddRange(loteAgendamentos);
            context.SaveChanges();

            lotesCriados += qtdParaEsteLote;
            Console.WriteLine($"-> Inseridos {lotesCriados} de {quantidadeAgendamentos} agendamentos...");
        }

        Console.WriteLine("Massa de dados gerada com sucesso!");
    }
}