using Xadrez.Models.Pecas;

namespace Xadrez.Models;

/// <summary>
/// Interface que representa um tabuleiro de xadrez.
/// </summary>
public interface ITabuleiro
{
    /// <summary>
    /// Obtém a casa com base nas coordenadas (linha e coluna) do tabuleiro. As coordenadas são baseadas em zero, ou seja, a primeira linha e coluna são 0.
    /// </summary>
    List<Casa> Casas { get; }

    /// <summary>
    /// Obtém quantidade de movimentos realizados na partida.
    /// </summary>
    int MovimentosExecutados { get; }

    /// <summary>
    /// Cria e distribui as peças no tabuleiro.
    /// </summary>
    void DistribuiPecas();

    /// <summary>
    /// Valida se o movimento é válido.
    /// </summary>
    /// <param name="movimento">Movimento a ser validado.</param>
    /// <returns>Retorna verdadeiro se o movimento é válido, caso contrário, retorna falso.</returns>
    bool ValidaMovimento(Movimento movimento, Jogador? jogador = null);

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


    /// <summary>
    /// Obtém a casa com base nas coordenadas (linha e coluna) do tabuleiro. As coordenadas são baseadas em zero, ou seja, a primeira linha e coluna são 0.
    /// </summary>
    /// <param name="linha">Linha da casa no tabuleiro. A linha é baseada em zero, ou seja, a primeira linha é 0.</param>
    /// <param name="coluna">Coluna da casa no tabuleiro. A coluna é baseada em zero, ou seja, a primeira coluna é 0.</param>
    /// <returns>Retorna a casa correspondente às coordenadas fornecidas. Se a casa não existir (fora dos limites do tabuleiro), retorna nulo.</returns>
    Casa? ObtemCasaCoordenadas(int linha, int coluna);

    /// <summary>
    /// Obtém movimentos quem mantém o rei em xeque.
    /// </summary>
    /// <param name="eBranca">Indica se o jogador é branco ou preto. Se for verdadeiro, o jogador é branco. Se for falso, o jogador é preto.</param>
    /// <returns>Uma lista de movimentos que mantém o rei em xeque.</returns>
    List<Movimento> ObtemMovimentosAtaqueCheque(bool eBranca);    
}