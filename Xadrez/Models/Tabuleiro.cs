using Xadrez.Models.Pecas;

namespace Xadrez.Models;

/// <summary>
/// Representa o tabuleiro de xadrez.
/// </summary>
public class Tabuleiro : ITabuleiro
{
    /// <summary>
    /// Representa o tamanho do tabuleiro de xadrez.
    /// </summary>
    public List<Casa> Casas { get; } = new();

    /// <summary>
    /// Pecas das partida
    /// </summary>
    public List<IPeca> Pecas { get; set; } = new();

    /// <summary>
    /// Pecas brancas da partida
    /// </summary>
    public List<IPeca> PecasBrancas => Pecas.Where(p => p.EBranca).ToList();

    /// <summary>
    /// Pecas pretas da partida
    /// </summary>
    public List<IPeca> PecasPretas => Pecas.Where(p => !p.EBranca).ToList();

    /// <summary>
    /// Representa a lista de peças capturadas durante a partida.
    /// </summary>
    public List<IPeca> PecasCapturadas { get; set; } = new();

    /// <summary>
    /// Representa a lista de peças brancas capturadas durante a partida.
    /// </summary>
    public List<IPeca> PecasBrancasCapturadas => PecasCapturadas.Where(p => p.EBranca).ToList();

    /// <summary>
    /// Representa a lista de peças pretas capturadas durante a partida.
    /// </summary>
    public List<IPeca> PecasPretasCapturadas => PecasCapturadas.Where(p => !p.EBranca).ToList();

    /// <summary>
    /// Construtor que inicializa o tabuleiro de xadrez com 64 casas.
    /// </summary>
    public Tabuleiro()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var casa = new Casa(i, j);
                Casas.Add(casa);
            }
        }
    }

    public void DistribuiPecas()
    {
        Pecas.Clear();

        foreach (var casa in Casas)
            casa.Peca = null;

        Casas.First(c => c.Linha == 0 && c.Coluna == 0).Peca = new Torre(true);
        Casas.First(c => c.Linha == 0 && c.Coluna == 1).Peca = new Cavalo(true);
        Casas.First(c => c.Linha == 0 && c.Coluna == 2).Peca = new Bispo(true);
        Casas.First(c => c.Linha == 0 && c.Coluna == 3).Peca = new Rainha(true);
        Casas.First(c => c.Linha == 0 && c.Coluna == 4).Peca = new Rei(true);
        Casas.First(c => c.Linha == 0 && c.Coluna == 5).Peca = new Bispo(true);
        Casas.First(c => c.Linha == 0 && c.Coluna == 6).Peca = new Cavalo(true);
        Casas.First(c => c.Linha == 0 && c.Coluna == 7).Peca = new Torre(true);

        for (int col = 0; col < 8; col++)
            Casas.First(c => c.Linha == 1 && c.Coluna == col).Peca = new Peao(true);


        Casas.First(c => c.Linha == 7 && c.Coluna == 0).Peca = new Torre(false);
        Casas.First(c => c.Linha == 7 && c.Coluna == 1).Peca = new Cavalo(false);
        Casas.First(c => c.Linha == 7 && c.Coluna == 2).Peca = new Bispo(false);
        Casas.First(c => c.Linha == 7 && c.Coluna == 3).Peca = new Rainha(false);
        Casas.First(c => c.Linha == 7 && c.Coluna == 4).Peca = new Rei(false);
        Casas.First(c => c.Linha == 7 && c.Coluna == 5).Peca = new Bispo(false);
        Casas.First(c => c.Linha == 7 && c.Coluna == 6).Peca = new Cavalo(false);
        Casas.First(c => c.Linha == 7 && c.Coluna == 7).Peca = new Torre(false);

        for (int col = 0; col < 8; col++)
            Casas.First(c => c.Linha == 6 && c.Coluna == col).Peca = new Peao(false);

        Pecas.AddRange(Casas.Where(c => c.Peca != null).Select(c => c.Peca!));
    }

    public bool ValidaMovimento(Movimento movimento)
    {
        var movimentosValidos = movimento.Peca.MovimentosPossiveis(this);
        return movimentosValidos.Any(m => m.CasaDestino == movimento.CasaDestino);
    }

    public void ExecutaMovimento(Movimento movimento)
    {
        movimento.CasaOrigem.Peca = null;

        if (movimento.PecaCapturada != null)
            Pecas.Remove(movimento.PecaCapturada);

        movimento.CasaDestino.Peca = movimento.Peca;
        movimento.Peca.FoiMovimentada = true;
    }

    public void ReverteMovimento(Movimento movimento)
    {
        movimento.CasaDestino.Peca = movimento.PecaCapturada;
        movimento.CasaOrigem.Peca = movimento.Peca;

        if (movimento.PecaCapturada != null)
            Pecas.Add(movimento.PecaCapturada);

        movimento.Peca.FoiMovimentada = false; 
    }

    public Casa? ObtemCasaPeca(IPeca peca)
    {
        return Casas.FirstOrDefault(c => c.Peca == peca);
    }

    public bool VerificaXeque(bool eBranca)
    {
        throw new NotImplementedException();
    }

    public bool VerificaXequeMate(bool eBranca)
    {
        var rei = Pecas.OfType<IRei>().FirstOrDefault(r => r.EBranca == eBranca);
        if (rei == null || !rei.EmCheque)
            return false;

        var pecasAliadas = Pecas.Where(p => p.EBranca == eBranca);
        foreach (var peca in pecasAliadas)
        {
            var movimentos = peca.MovimentosPossiveis(this);
            foreach (var mov in movimentos)
            {
                ExecutaMovimento(mov);
                bool continuaEmCheque = ((IRei)rei).EmCheque;
                ReverteMovimento(mov);

                if (!continuaEmCheque)
                    return false;
            }
        }

        return true;
    }

    public bool VerificaPerigo(Casa casa, bool eBranca)
    {
        throw new NotImplementedException();
    }

    public bool ValidaMovimento(Jogador jogador, Movimento movimento)
    {
        throw new NotImplementedException();
    }
}