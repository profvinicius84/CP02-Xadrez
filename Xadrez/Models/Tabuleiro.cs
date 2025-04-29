using Xadrez.Models.Pecas;
using System.Linq; // Adicionado para FirstOrDefault() e Any()
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
        // ... (outras validações como: é a vez do jogador? a peça é dele?) ...
        // (isso seria um caso futuro muito específico e foge um pouco do CP ou da parte do CP destinada a meu grupo)
        
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
        //    A expressão lambda (movLegal => ...) define a condição de busca.
        //    Ela verifica se existe ALGUM 'movLegal' na lista 'movimentosLegais'
        //    onde a CasaDestino E a PecaCapturada coincidem com as do 'movimento' proposto.
        bool movimentoValidoPelaPeca = movimentosLegais.Any(movLegal =>
                                                 movLegal.CasaDestino == destino && // 'destino' é movimento.CasaDestino
                                                 movLegal.PecaCapturada == movimento.PecaCapturada // Compara peças capturadas (ou null = null)
                                                 );

        // Se Any() retornou false, significa que nenhum movimento legal corresponde ao proposto.
        if (!movimentoValidoPelaPeca)
        {
            return false; // O movimento proposto não é um dos movimentos possíveis da peça.
        }

        // ... (outras validações GERAIS do xadrez, como: o movimento deixa o próprio rei em xeque?) ...
        // (isso seria um caso futuro muito específico e foge um pouco do CP)

        // Se chegou até aqui e passou por outras validações gerais (a serem adicionadas),
        // o movimento é considerado válido por enquanto.
        return true;
    }

    public void ExecutaMovimento(Movimento movimento)
    {
        IPeca peca = movimento.Peca;
        Casa casaOrigem = movimento.CasaOrigem;
        Casa casaDestino = movimento.CasaDestino;
        IPeca? pecaCapturada = movimento.PecaCapturada; // Pode ser null. Vai receber a peça (se tiver) ou null (se não tiver)
        
        if (pecaCapturada != null) // Se a houve peça capturada
        {
            // Adiciona a peça capturada à lista de capturadas
            PecasCapturadas.Add(pecaCapturada);

            // Remove a peça capturada da lista principal de peças no tabuleiro
            Pecas.Remove(pecaCapturada);
            // Nota: A peça capturada já está "fora" da casaDestino logicamente,
            // pois a peça que se move vai ocupar essa casa.
        }

        // Coloca a peça que se moveu na casa de destino
        casaDestino.Peca = peca;

        // Remove a peça da casa de origem (deixa vazia)
        casaOrigem.Peca = null;

        peca.FoiMovimentada = true;

        // Lógica para varificar se o movimento é o roque pode ser adicionada futuramente pelo grupo responsável
    }

    public void ReverteMovimento(Movimento movimento) // inverso de ExecutaMovimento
    {
        IPeca peca = movimento.Peca; // A peça que FOI movida
        Casa casaOrigem = movimento.CasaOrigem; // Para onde ela vai voltar
        Casa casaDestino = movimento.CasaDestino; // De onde ela vai sair
        IPeca? pecaCapturada = movimento.PecaCapturada; // A peça que FOI capturada (se houve)

        // Coloca a peça de volta na casa de origem
        casaOrigem.Peca = peca;

        // Limpa a casa de destino (ela fica vazia ou com a peça capturada)
        casaDestino.Peca = null; // Inicialmente limpamos, a peça capturada volta no próximo passo

        if (pecaCapturada != null) // Houve peça capturada
        {
            // Coloca a peça capturada de volta na casa de destino
            casaDestino.Peca = pecaCapturada;
            
            // Remove a peça da lista de capturadas
            PecasCapturadas.Remove(pecaCapturada);

            // Adiciona a peça de volta à lista de peças ativas no tabuleiro
            Pecas.Add(pecaCapturada);

            // Seria interessante restaurar o estado de FoiMovimentada caso não tenha movimentos anteriores
            // ao movimneto revertido, mas isso  exigiria passar a pilha de movimentos ou a partida inteira
            // como parâmetro, o que complica a assinatura do método ITabuleiro
            // Como meu grupo ficou responsável pelo meovimento do cavalo não faz sentido ter preocupação com isso
            // Pois é um refinamento específico futuro e afeta peças que estão no controle de outros grupos

            // Lógica para varificar se o movimento é o roque pode ser adicionada futuramente pelo grupo responsável
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