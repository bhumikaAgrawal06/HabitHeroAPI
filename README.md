# 🦸 HabitHero

HabitHero is a simple habit-tracking application built with a **.NET 9 Web API** backend and a **Streamlit** UI frontend.  
It helps you create, view, and complete habits while tracking your streaks — perfect for building consistency!

---

## 🚀 Features
- ✅ Create and manage habits  
- 📅 Track streaks when completing habits  
- 🖥️ REST API built with ASP.NET Core & Entity Framework (In-Memory DB)  
- 🎨 UI powered by Streamlit (Python)  

---

## 🏗️ Tech Stack
- **Backend**: .NET 9, ASP.NET Core, Entity Framework  
- **Frontend**: Streamlit (Python)  
- **Database**: In-Memory Database (EF Core)  
- **Version Control**: Git & GitHub  

---

## 📂 Project Structure
```text
HabitHeroAPI/
│── Controllers/        # API endpoints
│── Data/               # EF Core DbContext
│── Models/             # Habit model
│── Program.cs          # Entry point
│── ui/streamlit/       # Streamlit frontend
│    ├── habit_hero_streamlit.py
│    ├── requirements.txt
```
---

## 🔧 Setup & Run

```bash
git clone https://github.com/<your-username>/HabitHeroAPI.git
cd HabitHeroAPI
dotnet run
cd ui/streamlit
pip install -r requirements.txt
streamlit run habit_hero_streamlit.py
```

---

## 📖 API Endpoints
| Method | Endpoint        | Description        |
| ------ | --------------- | ------------------ |
| GET    | /api/habit      | Get all habits     |
| GET    | /api/habit/{id} | Get habit by ID    |
| POST   | /api/habit      | Create a new habit |
| PUT    | /api/habit/{id} | Update a habit     |
| DELETE | /api/habit/{id} | Delete a habit     |
