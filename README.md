# APTX Hub - Mini Social Network

**APTX Hub** is a mini social network web application built with **ASP.NET Core MVC**.  
It simulates the core features of modern social platforms, offering a space where users can connect, post, and interact.

## Tech Stack

- **Backend**: ASP.NET Core MVC (.NET 8)
- **Frontend**: Razor Views + Tailwind CSS
- **Database**: SQL Server
- **Authentication**: Identity + Google OAuth
- **Features**:
  - User registration & login
  - Post creation and editing
  - Commenting system
  - Save & like posts
  - Dark mode toggle
  - Responsive UI

## Getting Started

1. Clone the repo  
2. Configure `appsettings.json` with your DB connection  
3. Run migrations and start the server 
- dotnet ef database update --project APTXHub.Infrastructure --startup-project APTXHub
4. Open `https://localhost:5001` in your browser