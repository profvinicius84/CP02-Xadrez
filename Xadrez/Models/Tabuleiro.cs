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
        // Exemplo: adicionar dois cavalos, um branco e um preto
        AdicionaPeca(new Cavalo(true), 0, 1);  // Branco (B)
        AdicionaPeca(new Cavalo(true), 0, 6);  // Branco (B)
        AdicionaPeca(new Cavalo(false), 7, 1); // Preto (P)
        AdicionaPeca(new Cavalo(false), 7, 6); // Preto (P)
    }

    private void AdicionaPeca(IPeca peca, int linha, int coluna)
    {
        Casa? casa = Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == coluna);
        if (casa != null)
        {
            casa.Peca = peca;
            Pecas.Add(peca);
        }
    }

    public bool ValidaMovimento(Jogador jogador, Movimento movimento)
    {
        throw new NotImplementedException();
    }

    public void ExecutaMovimento(Movimento movimento)
    {
        throw new NotImplementedException();
    }

    public void ReverteMovimento(Movimento movimento)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public bool VerificaPerigo(Casa casa, bool eBranca)
    {
        throw new NotImplementedException();
    }
}