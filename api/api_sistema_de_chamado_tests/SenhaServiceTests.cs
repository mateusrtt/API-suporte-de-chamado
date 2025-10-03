using Application.Services;

namespace Api_tests
{
    public class SenhaServiceTests
    {
        private readonly SenhaService _senhaService;

        public SenhaServiceTests()
        {
            _senhaService = new SenhaService();
        }

        [Theory]
        [InlineData("senha123")]
        [InlineData("minhaSenha@2025")]
        [InlineData("Senha!Com#Caracteres$Especiais")]
        [InlineData("1234567890")]
        [InlineData("senha com espaço")]
        public void CriarSenha_Deve_Gerar_Hash_e_Salt(string senha)
        {
            // Act
            _senhaService.CriarSenhaHash(senha, out byte[] hash, out byte[] salt);

            // Assert
            Assert.NotNull(hash);
            Assert.NotNull(salt);
            Assert.NotEmpty(hash);
            Assert.NotEmpty(salt);
            Assert.NotEqual(hash, salt);
            Assert.Equal(64, hash.Length);   
            Assert.Equal(128, salt.Length);
        }

        [Theory]
        [InlineData("senha123")]
        [InlineData("TesteSeguranca")]
        [InlineData("123456789")]
        [InlineData("SenhaCom!Simbolo")]
        public void Verifica_SenhaHash_Deve_Retornar_True_Para_SenhaCorreta(string senha)
        {
            // Arrange
            _senhaService.CriarSenhaHash(senha, out byte[] hash, out byte[] salt);

            // Act
            bool resultado = _senhaService.VerificaSenhaHash(senha, hash, salt);

            // Assert
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("senha123", "outraSenha")]
        [InlineData("minhaSenhaSegura", "senha123")]
        [InlineData("senha com espaço", "senha sem espaço")]
        [InlineData("senhaMAIUSCULA", "senhamaIuscula")]
        public void Verifica_SenhaHash_Deve_Retornar_False_Para_SenhaErrada(string senhaCorreta, string senhaErrada)
        {
            // Arrange
            _senhaService.CriarSenhaHash(senhaCorreta, out byte[] hash, out byte[] salt);

            // Act
            bool resultado = _senhaService.VerificaSenhaHash(senhaErrada, hash, salt);

            // Assert
            Assert.False(resultado);
        }
    }
}