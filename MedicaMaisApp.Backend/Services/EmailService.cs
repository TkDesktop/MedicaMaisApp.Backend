using Resend;

namespace MedicaMaisApp.Backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IResend resend;

        public EmailService(IResend resend)
        {
            this.resend = resend;
        }

        public async Task EnviarCodigoConfirmacaoAsync(string email, string codigo)
        {
            var mensagem = new EmailMessage
            {
                From = "Medica+ <onboarding@resend.dev>",
                To = { email },
                Subject = "Confirmação de email - Medica+",
                HtmlBody = $"""
                 <!DOCTYPE html>
                 <html lang="pt-BR">
                 <head>
                     <meta charset="utf-8" />
                     <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                 </head>
                 <body style="margin:0; padding:0; background-color:#f4f6f8; font-family: 'Segoe UI', Arial, sans-serif;">
                     <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="background-color:#f4f6f8; padding:32px 0;">
                         <tr>
                             <td align="center">
                                 <table role="presentation" width="480" cellpadding="0" cellspacing="0" style="background-color:#ffffff; border-radius:12px;
                                 overflow:hidden; box-shadow:0 2px 8px rgba(0,0,0,0.06);">

                                     <!-- Header -->
                                     <tr>
                                         <td style="background-color:#0d9488; padding:24px 32px; text-align:center;">
                                             <span style="color:#ffffff; font-size:22px; font-weight:700; letter-spacing:0.5px;">
                                                 Medica+
                                             </span>
                                         </td>
                                     </tr>

                                     <!-- Body -->
                                     <tr>
                                         <td style="padding:40px 32px;">
                                             <h2 style="margin:0 0 12px 0; color:#111827; font-size:20px;">
                                                 Confirme seu email
                                             </h2>
                                             <p style="margin:0 0 24px 0; color:#4b5563; font-size:15px; line-height:1.5;">
                                                 Use o código abaixo para confirmar seu cadastro no Medica+. Ele é válido por
                                                 <strong>15 minutos</strong>.
                                             </p>

                                             <!-- Código destacado -->
                                             <table role="presentation" width="100%" cellpadding="0" cellspacing="0">
                                                 <tr>
                                                     <td align="center" style="padding:16px 0;">
                                                         <div style="display:inline-block; background-color:#f0fdfa; border:1px solid #0d9488; border-radius:8px; padding:16px 32px;">
                                                             <span style="font-size:32px; font-weight:700; letter-spacing:8px; color:#0d9488;">
                                                                 {codigo}
                                                             </span>
                                                         </div>
                                                     </td>
                                                 </tr>
                                             </table>

                                             <p style="margin:24px 0 0 0; color:#6b7280; font-size:13px; line-height:1.5;">
                                                 Se você não solicitou este cadastro, pode ignorar este email com segurança —
                                                 nenhuma ação será tomada.
                                             </p>
                                         </td>
                                     </tr>

                                     <!-- Footer -->
                                     <tr>
                                         <td style="background-color:#f9fafb; padding:20px 32px; text-align:center; border-top:1px solid #e5e7eb;">
                                             <p style="margin:0; color:#9ca3af; font-size:12px;">
                                                 © {DateTime.Now.Year} Medica+. Todos os direitos reservados.
                                             </p>
                                         </td>
                                     </tr>

                                 </table>
                             </td>
                         </tr>
                     </table>
                 </body>
                 </html>
                 """
            };

            await resend.EmailSendAsync(mensagem);
        }
    }
}