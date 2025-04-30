using System.Linq;
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

        // Helper para achar uma casa
        Casa GetCasa(int linha, int coluna) =>
            Casas.Single(c => c.Linha == linha && c.Coluna == coluna);

        //Peões brancos na linha 1
        for (int col = 0; col < 8; col++)
        {
            var p = new Peao(true);
            var casa = GetCasa(1, col);
            casa.Peca = p;
            Pecas.Add(p);
        }

        //Peças da linha 0 (torre, cavalo, bispo, dama, rei, bispo, cavalo, torre)
        IPeca[] ordemBackRankBranca = {
            new ITorre(true),  new Cavalo(true), new Bispo(true),  new Rainha(true),
            new Rei(true),    new Bispo(true),  new Cavalo(true), new Torre(true)
        };
        for (int col = 0; col < 8; col++)
        {
            var p = ordemBackRankBranca[col];
            var casa = GetCasa(0, col);
            casa.Peca = p;
            p.Casa = casa;
            Pecas.Add(p);
        }

        //Peões pretos na linha 6
        for (int col = 0; col < 8; col++)
        {
            var p = new Piao(false);
            var casa = GetCasa(6, col);
            casa.Peca = p;
            p.Casa = casa;
            Pecas.Add(p);
        }

        //Peças da linha 7 (igual à branca, mas com cor preta)
        IPeca[] ordemBackRankPreta = {
            new Torre(false),  new Cavalo(false), new Bispo(false),  new Rainha(false),
            new Rei(false),    new Bispo(false),   new Cavalo(false), new Torre(false)
        };
        for (int col = 0; col < 8; col++)
        {
            var p = ordemBackRankPreta[col];
            var casa = GetCasa(7, col);
            casa.Peca = p;
            p.Casa = casa;
            Pecas.Add(p);
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
        var rei = (eBranca ? PecasBrancas : PecasPretas).FirstOrDefault(p => p is IRei); //verifica se é branca ou preta e pega as peças certas
        if (rei == null) return false; // caso não encontre o rei

        var casaRei = ObtemCasaPeca(rei);
        var inimigos = eBranca ? PecasPretas : PecasBrancas;

        return inimigos.Any(inimigo =>
            inimigo.MovimentosPossiveis(this).Any(m => m.CasaDestino == casaRei)
        );
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