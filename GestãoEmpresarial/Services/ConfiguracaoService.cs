using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Services
{
    public class ConfiguracaoService
    {
        private const string RegistryPath = @"SOFTWARE\GestaoEmpresarial";

        #region Credenciais

        public void SalvarCredenciais(string usuario, string senha)
        {
            try
            {
                using (var key = Registry.CurrentUser.CreateSubKey(RegistryPath))
                {
                    if (key != null)
                    {
                        key.SetValue("Usuario", usuario);
                        key.SetValue("Senha", Encrypt(senha));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar credenciais: {ex.Message}");
            }
        }

        public (string usuario, string senha)? CarregarCredenciais()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RegistryPath))
                {
                    if (key != null)
                    {
                        string usuario = key.GetValue("Usuario") as string ?? "";
                        string senhaCriptografada = key.GetValue("Senha") as string ?? "";

                        if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(senhaCriptografada))
                        {
                            return (usuario, Decrypt(senhaCriptografada));
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public void LimparCredenciais()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RegistryPath, true))
                {
                    if (key != null)
                    {
                        key.DeleteValue("Usuario", false);
                        key.DeleteValue("Senha", false);
                    }
                }
            }
            catch { }
        }

        #endregion

        #region Tema

        // NOVO: Método para salvar a preferência do tema
        public void SalvarTema(bool isDarkTheme)
        {
            try
            {
                using (var key = Registry.CurrentUser.CreateSubKey(RegistryPath))
                {
                    if (key != null)
                    {
                        // Salva "Dark" ou "Light" para clareza no registro
                        key.SetValue("Theme", isDarkTheme ? "Dark" : "Light");
                    }
                }
            }
            catch (Exception ex)
            {
                // Em uma aplicação real, você poderia logar este erro
                Console.WriteLine($"Erro ao salvar tema: {ex.Message}");
            }
        }

        // NOVO: Método para carregar a preferência do tema
        public string CarregarTema()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RegistryPath))
                {
                    if (key != null)
                    {
                        // Carrega o valor, se não existir, retorna "Light" como padrão
                        return key.GetValue("Theme") as string ?? "Light";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar tema: {ex.Message}");
            }
            // Padrão caso a chave não exista ou dê erro
            return "Light";
        }

        #endregion

        #region Criptografia (sem alterações)

        private string Encrypt(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] encryptedBytes = ProtectedData.Protect(dataBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedBytes);
        }

        private string Decrypt(string encryptedData)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        #endregion
    }
}
