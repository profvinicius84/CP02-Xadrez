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
    public List<Casa> Casas { get; set; } = new();

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

    /// <summary>
    /// Distribui todas as peças nas posições iniciais do tabuleiro de xadrez.
    /// </summary>
    private void DistribuirPecas(List<Casa> Casas)
    {
        // Pretas
        Casas[0][0].Peca = new Torre(false);
        Casas[0][1].Peca = new Cavalo(false);
        Casas[0][2].Peca = new Bispo(false);
        Casas[0][3].Peca = new Rainha(false);
        Casas[0][4].Peca = new Rei(false);
        Casas[0][5].Peca = new Bispo(false);
        Casas[0][6].Peca = new Cavalo(false);
        Casas[0][7].Peca = new Torre(false);

        for (int coluna = 0; coluna < 8; coluna++)
        {
            Casas[1][coluna].Peca = new Peao(false);
        }

        // Brancas
        Casas[7][0].Peca = new Torre(true);
        Casas[7][1].Peca = new Cavalo(true);
        Casas[7][2].Peca = new Bispo(true);
        Casas[7][3].Peca = new Rainha(true);
        Casas[7][4].Peca = new Rei(true);
        Casas[7][5].Peca = new Bispo(true);
        Casas[7][6].Peca = new Cavalo(true);
        Casas[7][7].Peca = new Torre(true);

        for (int coluna = 0; coluna < 8; coluna++)
        {
            Casas[6][coluna].Peca = new Peao(true);
        }
    }

    /// <summary>
    /// Valida se um movimento é permitido no tabuleiro.
    /// </summary>
    /// <param name="movimento">Movimento a ser validado.</param>
    /// <returns>Retorna verdadeiro se o movimento é válido, caso contrário, falso.</returns>
    public bool ValidaMovimento(Movimento movimento)
    {
        if (movimento == null || movimento.CasaOrigem == null || movimento.CasaDestino == null)
            return false;

        var movimentosPossiveis = movimento.CasaOrigem.Peca?.MovimentosPossiveis(this);

        if (movimentosPossiveis == null)
            return false;

        foreach (var mov in movimentosPossiveis)
        {
            if (mov.CasaDestino == movimento.CasaDestino)
                return true;
        }

        return false;
    }


    /// <summary>
    /// Executa um movimento no tabuleiro.
    /// </summary>
    /// <param name="movimento">Movimento a ser executado.</param>
    public void ExecutaMovimento(Movimento movimento)
    {
        if (!ValidaMovimento(movimento))
            return;

        movimento.CasaDestino.Peca = movimento.CasaOrigem.Peca;
        movimento.CasaOrigem.Peca = null;
    }


    /// <summary>
    /// Reverte um movimento no tabuleiro (desfaz o último movimento).
    /// </summary>
    /// <param name="movimento">Movimento a ser revertido.</param>
    public void ReverteMovimento(Movimento movimento)
    {
        if (movimento == null)
            return;

        movimento.CasaOrigem.Peca = movimento.PecaMovida;
        movimento.CasaDestino.Peca = movimento.PecaCapturada;
    }


    /// <summary>
    /// Obtém a casa atual de uma peça no tabuleiro.
    /// </summary>
    /// <param name="peca">Peça a ser localizada.</param>
    /// <returns>A casa onde a peça se encontra, ou null se não encontrada.</returns>
    public Casa ObtemCasaPeca(Peca peca)
    {
        foreach (var linha in Casas)
        {
            foreach (var casa in linha)
            {
                if (casa.Peca == peca)
                    return casa;
            }
        }
        return null;
    }


    public bool VerificaXeque(bool eBranca)
    {
        throw new NotImplementedException();
    }

    public bool VerificaXequeMate(bool eBranca)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Verifica se uma casa está sob ataque.
    /// </summary>
    /// <param name="casa">Casa a ser verificada.</param>
    /// <param name="eBranca">Indica se o jogador é branco ou preto.</param>
    /// <returns>Retorna verdadeiro se a casa está sob ataque, caso contrário, falso.</returns>
    public bool VerificaPerigo(Casa casa, bool eBranca)
    {
        foreach (var linha in Casas)
        {
            foreach (var outraCasa in linha)
            {
                if (outraCasa.Peca != null && outraCasa.Peca.EBranca != eBranca)
                {
                    var movimentosPossiveis = outraCasa.Peca.MovimentosPossiveis(this);
                    foreach (var movimento in movimentosPossiveis)
                    {
                        if (movimento.Destino == casa)
                            return true;
                    }
                }
            }
        }
        return false;
    }

}