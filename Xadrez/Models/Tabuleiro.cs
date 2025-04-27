using Xadrez.Models.Pecas;
using System.Linq; // Adicionado para FirstOrDefault
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
        throw new NotImplementedException();
    }

    public bool ValidaMovimento(Jogador jogador, Movimento movimento)
    {
        //throw new NotImplementedException();
        // ... (outras validações como: é a vez do jogador? a peça é dele?) ...
        IPeca peca = movimento.Peca;
        Casa origem = movimento.CasaOrigem;
        Casa destino = movimento.CasaDestino;

        // Verifica se a peça existe na casa de origem indicada
        if (origem.Peca != peca)
        {
            return false; // Inconsistência, a peça não está onde o movimento diz que está
        }

        // 1. Calcula TODOS os movimentos possíveis para esta peça a partir da origem
        List<Movimento> movimentosLegais = peca.MovimentosPossiveis(this);

        // 2. Verifica se o movimento PROPOSTO está entre os movimentos LEGAIS calculados no passo anterior.
        bool movimentoEncontrado = false;
        // Continuar a partir do foreach...
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