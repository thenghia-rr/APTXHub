# APTX Hub - Mini Social Network

**APTX Hub** is a mini social network web application built with **ASP.NET Core**.  
It simulates the core features of modern social platforms, offering a space where users can connect, post, and interact.

## Tech Stack

- **Backend**: ASP.NET Core MVC (.NET 8)
- **Frontend**: Razor Views + Tailwind CSS
- **Database**: SQL Server
- **Authentication**: Identity + Google OAuth + GitHub
- **Real-time**: SignalR
- **Architecture**: Service-Oriented Architecture (SOA) 
- **Features**: 
  - User registration & login & update password/profile
  - Post creation and delete soft/hard, Reported
  - Commenting system
  - Save(favorite) & like posts
  - Hashtags by post
  - Create story (image/video)
  - Friends management
  - Search post smart
  - Notification
  - Dark mode toggle
  - Responsive UI

## Getting Started

1. Clone the repo  
2. Configure `appsettings.json` with your DB connection  
3. Run migrations and start the server 
  - dotnet ef database update --project APTXHub.Infrastructure --startup-project APTXHub
4. Open `https://localhost:7002` or `http://localhost:5046` in your browser 