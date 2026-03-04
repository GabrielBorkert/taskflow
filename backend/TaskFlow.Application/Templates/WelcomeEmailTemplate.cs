namespace TaskFlow.Application.Templates
{
    public static class WelcomeEmailTemplate
    {
        public static string Generate(string userName)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f3f4f6;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background-color: #ffffff;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 4px 20px rgba(0,0,0,0.1);
        }}
        .header {{
            background-color: #6366f1;
            padding: 32px;
            text-align: center;
        }}
        .header h1 {{
            color: white;
            margin: 0;
            font-size: 28px;
        }}
        .content {{
            padding: 32px;
        }}
        .content h2 {{
            color: #1e293b;
        }}
        .content p {{
            color: #64748b;
            line-height: 1.6;
        }}
        .button {{
            display: inline-block;
            margin-top: 24px;
            padding: 12px 32px;
            background-color: #6366f1;
            color: white;
            text-decoration: none;
            border-radius: 8px;
            font-weight: bold;
        }}
        .footer {{
            padding: 24px 32px;
            text-align: center;
            color: #94a3b8;
            font-size: 12px;
            border-top: 1px solid #e2e8f0;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>TaskFlow</h1>
        </div>
        <div class='content'>
            <h2>Bem-vindo, {userName}! 🎉</h2>
            <p>
                Estamos muito felizes em ter você no TaskFlow. 
                Sua conta foi criada com sucesso e você já pode começar a organizar suas tarefas.
            </p>
            <p>
                Com o TaskFlow você pode gerenciar suas tarefas de forma simples e eficiente, 
                acompanhando o progresso de cada uma delas no seu quadro Kanban.
            </p>
            <a href='http://localhost:4200' class='button'>Acessar o TaskFlow</a>
        </div>
        <div class='footer'>
            <p>Este email foi enviado automaticamente. Por favor, não responda.</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}