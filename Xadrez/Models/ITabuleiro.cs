using Xadrez.Models.Pecas;

namespace Xadrez.Models;

/// <summary>
/// Interface que representa um tabuleiro de xadrez.
/// </summary>
public interface ITabuleiro
{
    /// <summary>
    /// Cria e distribui as peças no tabuleiro.
    /// </summary>
    void DistribuiPecas();

    /// <summary>
    /// Valida se o movimento é válido.
    /// </summary>
    /// <param name="movimento">Movimento a ser validado.</param>
    /// <returns>Retorna verdadeiro se o movimento é válido, caso contrário, retorna falso.</returns>
    bool ValidaMovimento(Jogador jogador, Movimento movimento);

    /// <summary>
    /// Executa o movimento no tabuleiro. O movimento é composto por uma peça e uma casa de destino. O movimento pode ser um movimento normal ou um roque (pequeno ou grande).
    /// </summary>
    /// <param name="movimento">Movimento a ser executado.</param>
    void ExecutaMovimento(Movimento movimento);

    /// <summary>
    /// Reverte o movimento no tabuleiro. O movimento é composto por uma peça e uma casa de destino. O movimento pode ser um movimento normal ou um roque (pequeno ou grande).
    /// </summary>
    /// <param name="movimento">Movimento a ser revertido.</param>
    void ReverteMovimento(Movimento movimento);

    /// <summary>
    /// Obtém a casa onde a peça está localizada no tabuleiro.
    /// </summary>
    /// <param name="peca">Peça a ser localizada.</param>
    /// <returns>Retorna a casa onde a peça está localizada. Se a peça não estiver no tabuleiro, retorna nulo.</returns>
    Casa? ObtemCasaPeca(IPeca peca);

    /// <summary>
    /// Verifica se um jogador está em xeque. O xeque ocorre quando o rei do jogador está ameaçado por uma peça adversária. O xeque-mate ocorre quando o rei do jogador está ameaçado e não há movimentos possíveis para escapar do xeque.
    /// </summary>
    /// <param name="eBranca">Indica se o jogador é branco ou preto. Se for verdadeiro, o jogador é branco. Se for falso, o jogador é preto.</param>
    /// <returns>Retorna verdadeiro se o jogador está em xeque, caso contrário, retorna falso.</returns>
    bool VerificaXeque(bool eBranca);

    /// <summary>
    /// Verifica se um jogador está em xeque-mate. O xeque-mate ocorre quando o rei do jogador está ameaçado e não há movimentos possíveis para escapar do xeque.
    /// </summary>
    /// <param name="eBranca">Indica se o jogador é branco ou preto. Se for verdadeiro, o jogador é branco. Se for falso, o jogador é preto.</param>
    /// <returns>Retorna verdadeiro se o jogador está em xeque-mate, caso contrário, retorna falso.</returns>
    bool VerificaXequeMate(bool eBranca);

    /// <summary>
    /// Verifica se uma casa está sob ataque. Uma casa está sob ataque se uma peça adversária pode se mover para essa casa.
    /// </summary>
    /// <param name="casa">Casa a ser verificada.</param>
    /// <param name="eBranca">Indica se o jogador é branco ou preto. Se for verdadeiro, o jogador é branco. Se for falso, o jogador é preto.</param>
    /// <returns>Retorna verdadeiro se a casa está sob ataque, caso contrário, retorna falso.</returns>
    bool VerificaPerigo(Casa casa, bool eBranca);
}