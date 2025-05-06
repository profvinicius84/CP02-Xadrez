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

    /// <summary>
    /// Posiciona as torres brancas e pretas nas posições iniciais do tabuleiro,
    /// conforme o padrão do xadrez tradicional.
    /// </summary>
    public void DistribuiPecas()
    {
        // Coloca as torres nas posições iniciais
        Casas.First(c => c.Linha == 0 && c.Coluna == 0).Peca = new Torre(true);
        Casas.First(c => c.Linha == 0 && c.Coluna == 7).Peca = new Torre(true);

        Casas.First(c => c.Linha == 7 && c.Coluna == 0).Peca = new Torre(false);
        Casas.First(c => c.Linha == 7 && c.Coluna == 7).Peca = new Torre(false);
    }


    /// <summary>
    /// Verifica se o movimento feito é válido.
    /// </summary>
    /// <param name="jogador">O jogador que está tentando mover a peça.</param>
    /// <param name="tentativa">O movimento que o jogador deseja realizar.</param>
    /// <returns>Retorna true se o movimento for permitido, senão false.</returns>
    public bool ValidaMovimento(Jogador jogador, Movimento tentativa)
    {
        // Verifica se existe uma peça para mover
        if (tentativa.Peca == null)
        {
            return false;
        }

        // Garante que o jogador só possa mover as peças da sua própria cor
        if (tentativa.Peca.EBranca != jogador.EBranco)
        {
            return false;
        }

        // Lista todos os movimentos que a peça pode fazer
        var opcoes = tentativa.Peca.MovimentosPossiveis(this);

        // Verifica se o destino está dentro das opções válidas
        foreach (var mov in opcoes)
        {
            if (mov.CasaDestino.Linha == tentativa.CasaDestino.Linha &&
                mov.CasaDestino.Coluna == tentativa.CasaDestino.Coluna)
            {
                return true;
            }
        }

        return false;
    }



    /// <summary>
    /// Executa o movimento da peça no tabuleiro, movendo-a da casa de origem para a de destino.
    /// </summary>
    /// <param name="jogada">Movimento contendo a peça, origem e destino.</param>
    public void ExecutaMovimento(Movimento jogada)
    {
        var origem = jogada.CasaOrigem;
        var destino = jogada.CasaDestino;

        // Se houver uma peça no destino, significa que será capturada
        if (destino.Peca != null)
        {
            PecasCapturadas.Add(destino.Peca);
        }

        // Move a peça da casa de origem para a casa de destino
        destino.Peca = origem.Peca;

        // Limpa a casa de onde a peça saiu
        origem.Peca = null;

        // Marca que a peça já foi movimentada
        if (destino.Peca != null)
        {
            destino.Peca.FoiMovimentada = true;
        }
    }


    /// <summary>
    /// Reverte um movimento que foi feito
    /// </summary>
    /// <param name="jogada">O movimento a ser desfeito.</param>
    public void ReverteMovimento(Movimento jogada)
    {
        var origem = jogada.CasaOrigem;
        var destino = jogada.CasaDestino;

        // Devolve a peça para a casa de origem
        origem.Peca = destino.Peca;

        // Restaura a peça capturada, se houver, na casa de destino
        destino.Peca = jogada.PecaCapturada;

        // Marca que a peça voltou ao estado "nunca movimentada"
        if (origem.Peca != null)
        {
            origem.Peca.FoiMovimentada = false;
        }

        // Remove a peça da lista de capturadas, já que ela foi restaurada
        if (jogada.PecaCapturada != null)
        {
            PecasCapturadas.Remove(jogada.PecaCapturada);
        }
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