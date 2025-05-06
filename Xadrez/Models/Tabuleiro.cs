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
        PecasCapturadas.Clear();
        foreach (var casa in Casas)
            casa.Peca = null;

        // Peças brancas (Por enquanto Só tem torres e rei para o movimento roque)
        int linha = 0;
        ColocaPeca(new Torre(true), linha, 0);
        //ColocaPeca(new Hades(true), linha, 1);
        //ColocaPeca(new Hades(true), linha, 2);
        ColocaPeca(new Rei(true), linha, 3);
        //ColocaPeca(new Hades(true), linha, 4);
        //ColocaPeca(new Hades(true), linha, 5);
        //ColocaPeca(new Hades(true), linha, 6);
        ColocaPeca(new Torre(true), linha, 7);

        // Peças pretas (Por enquanto Só tem torres e rei para o movimento roque)
        linha = 7;
        ColocaPeca(new Torre(false), linha, 0);
        //ColocaPeca(new Hades(false), linha, 1);
        //ColocaPeca(new Hades(false), linha, 2);
        ColocaPeca(new Rei(false), linha, 3);
        //ColocaPeca(new Hades(false), linha, 4);
        //ColocaPeca(new Hades(false), linha, 5);
        //ColocaPeca(new Hades(false), linha, 6);
        ColocaPeca(new Torre(false), linha, 7);
    }

    public bool ValidaMovimento(Jogador jogador, Movimento movimento)
    {
        if (movimento.Peca.Codigo == "R")
        {
            // Verifica se a peça pertence ao jogador
            if (movimento.Peca.EBranca != jogador.EBranco)
                return false;

            // Obtém todos os movimentos possíveis para a peça
            var possiveis = movimento.Peca.MovimentosPossiveis(this);

            // Verifica se o movimento está na lista de movimentos possíveis
            if (!possiveis.Any(m => m.CasaOrigem == movimento.CasaOrigem && m.CasaDestino == movimento.CasaDestino))
                return false;

            ExecutaMovimento(movimento);
            ReverteMovimento(movimento);
            return true;
        }

        return false;
    }

    public void ExecutaMovimento(Movimento movimento)
    {
        // Processa captura de peça, se houver
        if (movimento.PecaCapturada != null)
        {
            Pecas.Remove(movimento.PecaCapturada);
            PecasCapturadas.Add(movimento.PecaCapturada);
        }

        // Move a peça para a casa de destino
        movimento.CasaOrigem.Peca = null;
        movimento.CasaDestino.Peca = movimento.Peca;

        // Marca a peça como movimentada (importante para roque e peões)
        movimento.Peca.FoiMovimentada = true;
    }

    public void ReverteMovimento(Movimento movimento)
    {
        // Retorna a peça para a casa de origem
        movimento.CasaOrigem.Peca = movimento.Peca;

        // Restaura a peça capturada, se houver
        movimento.CasaDestino.Peca = movimento.PecaCapturada;

        // Se houve captura, restaura a peça capturada
        if (movimento.PecaCapturada != null)
        {
            PecasCapturadas.Remove(movimento.PecaCapturada);
            Pecas.Add(movimento.PecaCapturada);
        }
        movimento.Peca.FoiMovimentada = false;
    }

    public Casa? ObtemCasaPeca(IPeca peca)
    {
        return Casas.FirstOrDefault(c => c.Peca == peca);
    }

    private void ColocaPeca(IPeca peca, int linha, int coluna)
    {
        var casa = Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == coluna);
        if (casa != null)
        {
            casa.Peca = peca;
            Pecas.Add(peca);
        }
    }

    public bool VerificaXeque(bool eBranca)
    {
        throw new NotImplementedException();
    }

    public bool VerificaXequeMate(bool eBranca)
    {
        throw new NotImplementedException();
    }

    public bool VerificaPerigo(Casa casa, bool eBranca)
    {
        throw new NotImplementedException();
    }
}