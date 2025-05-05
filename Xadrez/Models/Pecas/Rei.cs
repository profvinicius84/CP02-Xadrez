using Xadrez.Models;

namespace Xadrez.Models.Pecas;

/// <summary>
/// Representa o rei no jogo de xadrez.
/// </summary>
public class Rei : IPeca, IRei
{
    /// <summary>
    /// Indica se o rei está em xeque.
    /// </summary>
    public bool EmCheque { get; set; }

    /// <summary>
    /// Indica se a peça é branca.
    /// </summary>
    public bool EBranca { get; }

    /// <summary>
    /// Indica se a peça já foi movimentada.
    /// </summary>
    public bool FoiMovimentada { get; set; }

    /// <summary>
    /// Construtor da classe Rei.
    /// </summary>
    /// <param name="eBranca">Indica se a peça é branca.</param>
    public Rei(bool eBranca)
    {
        EBranca = eBranca;
        FoiMovimentada = false;
        EmCheque = false;
    }

    /// <summary>
    /// Obtém os movimentos possíveis para o rei.
    /// </summary>
    /// <param name="tabuleiro">Tabuleiro onde o rei está.</param>
    /// <returns>Retorna uma lista de movimentos possíveis.</returns>
    public List<Movimento> MovimentosPossiveis(ITabuleiro tabuleiro)
    {
        var movimentos = new List<Movimento>();
        var casaAtual = tabuleiro.ObtemCasaPeca(this);
        if (casaAtual == null)
            return movimentos;

        int[] direcoesLinha = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] direcoesColuna = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < 8; i++)
        {
            int novaLinha = casaAtual.Linha + direcoesLinha[i];
            int novaColuna = casaAtual.Coluna + direcoesColuna[i];

            if (novaLinha >= 0 && novaLinha < 8 && novaColuna >= 0 && novaColuna < 8)
            {
                var casaDestino = tabuleiro.Casas.First(c => c.Linha == novaLinha && c.Coluna == novaColuna);
                if (casaDestino.Peca == null || casaDestino.Peca.EBranca != EBranca)
                {
                    movimentos.Add(new Movimento(this, casaAtual, casaDestino));
                }
            }
        }

        return movimentos;
    }

    /// <summary>
    /// Verifica se o roque é válido.
    /// </summary>
    /// <param name="tabuleiro">Tabuleiro onde o roque será realizado.</param>
    /// <param name="roquePequeno">Indica se é um roque pequeno (true) ou grande (false).</param>
    /// <returns>Retorna verdadeiro se o roque é válido, caso contrário, retorna falso.</returns>
    public bool VerificaRoque(ITabuleiro tabuleiro, bool roquePequeno)
    {
        if (FoiMovimentada || EmCheque)
            return false;

        var casaRei = tabuleiro.ObtemCasaPeca(this);
        if (casaRei == null)
            return false;

        int colunaTorre = roquePequeno ? 7 : 0;
        var casaTorre = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaTorre);
        if (casaTorre.Peca == null || !(casaTorre.Peca is ITorre) || casaTorre.Peca.EBranca != EBranca || casaTorre.Peca.FoiMovimentada)
            return false;

        int direcao = roquePequeno ? 1 : -1;
        int colunaAtual = casaRei.Coluna + direcao;
        while (colunaAtual != colunaTorre)
        {
            var casa = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaAtual);
            if (casa.Peca != null)
                return false;

            if (tabuleiro.VerificaPerigo(casa, EBranca))
                return false;

            colunaAtual += direcao;
        }

        return true;
    }

    /// <summary>
    /// Executa o roque.
    /// </summary>
    /// <param name="tabuleiro">Tabuleiro onde o roque será realizado.</param>
    /// <param name="roquePequeno">Indica se é um roque pequeno (true) ou grande (false).</param>
    public void ExecutaRoque(ITabuleiro tabuleiro, bool roquePequeno)
    {
        var casaRei = tabuleiro.ObtemCasaPeca(this);
        if (casaRei == null)
            return;

        int colunaTorre = roquePequeno ? 7 : 0;
        var casaTorre = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaTorre);
        if (casaTorre.Peca == null || !(casaTorre.Peca is ITorre))
            return;

        int colunaReiDestino = roquePequeno ? 6 : 2;
        int colunaTorreDestino = roquePequeno ? 5 : 3;

        var casaReiDestino = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaReiDestino);
        var casaTorreDestino = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaTorreDestino);

        casaRei.Peca = null;
        casaTorre.Peca = null;
        casaReiDestino.Peca = this;
        casaTorreDestino.Peca = casaTorre.Peca;

        FoiMovimentada = true;
        casaTorre.Peca.FoiMovimentada = true;
    }
} 