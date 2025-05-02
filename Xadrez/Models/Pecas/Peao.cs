namespace Xadrez.Models.Pecas
{
    /// <summary>
    /// Representa um peão no jogo de xadrez.
    /// Herda da classe base <see cref="Peca"/> e implementa a interface <see cref="IPeao"/>.
    /// </summary>
    public class Peao : Peca, IPeao
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Peao"/>.
        /// </summary>
        /// <param name="eBranca">Indica se o peão é branco (true) ou preto (false).</param>
        public Peao(bool eBranca) : base(eBranca) { }

        /// <summary>
        /// Devolve os movimentos possíveis para o peão.
        /// </summary>
        /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
        /// <returns>Uma lista de movimentos possíveis para o peão.</returns>
        public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
        {
            var movimentos = new List<Movimento>();

            var casaAtual = tabuleiro.ObtemCasaPeca(this);
            if (casaAtual is null)
            {
                return movimentos;
            }

            int direcao = EBranca ? 1 : -1;
            int linhaAtual = casaAtual.Linha;
            int colunaAtual = casaAtual.Coluna;

            // 1 casa para frente
            var destino1 = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linhaAtual + direcao && c.Coluna == colunaAtual);
            if (destino1 is not null && destino1.Peca is null)
            {
                movimentos.Add(new Movimento(this, casaAtual, destino1));

                // 2 casas para frente (apenas se o peão ainda não foi movimentado)
                bool naLinhaInicial = (EBranca && linhaAtual == 1) || (!EBranca && linhaAtual == 6);
                var destino2 = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linhaAtual + 2 * direcao && c.Coluna == colunaAtual);
                if (naLinhaInicial && destino2 is not null && destino2.Peca is null)
                {
                    movimentos.Add(new Movimento(this, casaAtual, destino2));
                }
            }

            // Captura na diagonal esquerda
            var diagonalEsquerda = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linhaAtual + direcao && c.Coluna == colunaAtual - 1);
            if (diagonalEsquerda?.Peca is not null && diagonalEsquerda.Peca.EBranca != this.EBranca)
            {
                movimentos.Add(new Movimento(this, casaAtual, diagonalEsquerda, diagonalEsquerda.Peca));
            }

            // Captura na diagonal direita
            var diagonalDireita = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linhaAtual + direcao && c.Coluna == colunaAtual + 1);
            if (diagonalDireita?.Peca is not null && diagonalDireita.Peca.EBranca != this.EBranca)
            {
                movimentos.Add(new Movimento(this, casaAtual, diagonalDireita, diagonalDireita.Peca));
            }

            return movimentos;
        }

        /// <summary>
        /// Verifica se o peão pode ser promovido.
        /// </summary>
        /// <param name="tabuleiro">O tabuleiro atual.</param>
        /// <param name="casaDestino">A casa onde o peão será movido.</param>
        /// <returns>True se puder promover; caso contrário, false.</returns>
        public bool VerificaPromocao(Tabuleiro tabuleiro, Casa casaDestino)
        {
            if (casaDestino.Linha == (EBranca ? 7 : 0))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Promove o peão para outra peça (dama, torre, bispo ou cavalo),
        /// substituindo-o na casa do tabuleiro e atualizando a lista de peças.
        /// </summary>
        /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
        public void Promover(Tabuleiro tabuleiro)
        {
            var casa = tabuleiro.ObtemCasaPeca(this);

            if (!VerificaPromocao(tabuleiro, casa))
            {
                Console.WriteLine("O peão ainda não pode ser promovido.\n");
                return;
            }
            else
            {
                // Solicita a escolha do usuário
                Console.WriteLine("Você atingiu a última linha. Escolha uma promoção:\nD - Dama\nT - Torre\nB - Bispo\nC - Cavalo");
                string escolha = Console.ReadLine()?.ToUpper() ?? "D";

                PecaPromocao = escolha switch
                {
                    "T" => new Hades(EBranca),
                    "B" => new Hades(EBranca),
                    "C" => new Hades(EBranca),
                    _ => new Hades(EBranca), // padrão
                };

                // Substitui o peão pela nova peça
                casa.Peca = PecaPromocao;
                tabuleiro.Pecas.Remove(this);
                tabuleiro.Pecas.Add(PecaPromocao);
                Console.WriteLine($"Peão promovido a {PecaPromocao.GetType().Name}!");
            }
        }

        /// <summary>
        /// A nova peça para a qual o peão foi promovido.
        /// </summary>
        public IPeca PecaPromocao { get; set; }
    }
}
