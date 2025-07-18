# APTX Hub - Mini Social Network

**APTX Hub** is a modern, lightweight social networking platform built with **ASP.NET Core**.
It replicates essential social features, allowing users to connect, post, and engage in real-time.

## Tech Stack

| Layer        | Technology                          |
| ------------ | ----------------------------------- |
| Backend      | ASP.NET Core MVC (.NET 8)           |
| Frontend     | Razor Views + Tailwind CSS          |
| Database     | SQL Server, Cloudinary (video/image)|
| Auth         | ASP.NET Identity + Google + GitHub  |
| Realtime     | SignalR                             |
| Architecture | Service-Oriented Architecture (SOA) |

## Features

* User Registration, Login, Profile & Password Update
* OAuth with Google & GitHub
* Post: create, soft/hard delete, report system
* Comments on posts
* Like & Save posts
* Hashtag support
* Stories: video/image in 24hours
* Reels: short video/image feed
* Friend requests & management
* Real-time notifications
* Search: users & posts
* Light/Dark mode toggle
* Multi-language support (EN/VI)
* Real-time Chat
* Verified Badge
* Fully responsive design
* Admin dashboard for moderation
* Cloudinary for storage image/video

## Screenshots

* Homepage (overview feed + suggested friends + trending)
* User detail page
* Chat (real-time with friends)
* Reels (short video feature)


#### Homepage
![](APTXHub/wwwroot/images/docs/home.png)

#### Homepage
![](APTXHub/wwwroot/images/docs/user_detail.png)

#### Real-time Chat
![](APTXHub/wwwroot/images/docs/chat.png)

#### Reels Feature
![](APTXHub/wwwroot/images/docs/reels.png)


## Getting Started

```bash
# 1. Clone the repository
git clone https://github.com/thenghia-rr/APTXHub.git

# 2. Navigate into the project
cd APTXHub

# 3. Configure appsettings.json (DB connection, auth keys...)

# 4. Run EF Core Migration
dotnet ef database update --project APTXHub.Infrastructure --startup-project APTXHub

# 5. Launch the app
dotnet run --project APTXHub
```

Visit:
[https://localhost:7002](https://localhost:7002) or [http://localhost:5046](http://localhost:5046)

## Admin Access (Demo)

```
Email: admin@gmail.com
Password: Susu@123
```