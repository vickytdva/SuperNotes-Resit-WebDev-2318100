# SuperNotes Application

A full-stack note-taking application built with .NET Core backend and React frontend. This application allows users to create, manage, and organize their notes with a simple and intuitive interface.

## Features

- 🔐 User Authentication
- 📝 Create and manage notes
- 🔍 Search notes by title
- 🗑️ Delete notes
- 💾 Persistent storage with SQLite
- 🎨 Clean and intuitive user interface

## Tech Stack

### Backend
- .NET Core 9.0
- Entity Framework Core
- SQLite Database
- Cookie-based Authentication
- ASP.NET Core Web API

### Frontend
- React.js
- Axios for API calls
- Modern JavaScript (ES6+)
- CSS for styling

## Getting Started

### Prerequisites
- .NET Core SDK 9.0 or later
- Node.js and npm
- Git

### Backend Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/vickytdva/SuperNotes-Resit-WebDev-2318100.git
   cd SuperNotes-Resit-WebDev-2318100
   ```

2. Navigate to the backend directory and restore packages:
   ```bash
   dotnet restore
   ```

3. Run the migrations:
   ```bash
   dotnet ef database update
   ```

4. Start the backend server:
   ```bash
   dotnet run
   ```
   The backend will start on `https://localhost:7159` and `http://localhost:5091`

### Frontend Setup
1. Navigate to the frontend directory:
   ```bash
   cd supernotesfrontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the frontend development server:
   ```bash
   npm start
   ```
   The frontend will start on `http://localhost:3001`

## Usage

1. Open your browser and navigate to `http://localhost:3001`
2. Enter any username to log in (no password required for this demo)
3. Start creating and managing your notes!

## API Endpoints

### Authentication
- `POST /auth/login` - Login with username
- `POST /auth/logout` - Logout user
- `GET /auth/check` - Check authentication status

### Notes
- `GET /api/notes` - Get all notes for authenticated user
- `POST /api/notes` - Create a new note
- `DELETE /api/notes/{id}` - Delete a note
- `GET /api/notes/search` - Search notes by title

## Security Features

- Cookie-based authentication
- CORS configuration for security
- HTTP-only cookies
- Secure cookie policies
- User-specific note access

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Author

Viktoria Todorova - [GitHub Profile](https://github.com/vickytdva) 