import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Notes = ({ user }) => {
  const [notes, setNotes] = useState([]);
  const [newNote, setNewNote] = useState({ title: '', content: '' });
  const [searchQuery, setSearchQuery] = useState('');
  const [filteredNotes, setFilteredNotes] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchNotes();
  }, [user]);

  const fetchNotes = async () => {
    try {
      const response = await axios.get('https://localhost:7159/api/notes');
      setNotes(response.data);
      setFilteredNotes(response.data);
    } catch (err) {
      console.error('Failed to fetch notes:', err);
      setError('Failed to fetch notes');
    }
  };

  const handleSearch = (event) => {
    setSearchQuery(event.target.value);
    if (event.target.value === '') {
      setFilteredNotes(notes);
    } else {
      const filtered = notes.filter((note) =>
        note.title.toLowerCase().includes(event.target.value.toLowerCase())
      );
      setFilteredNotes(filtered);
    }
  };

  const handleAddNote = async () => {
    if (newNote.title && newNote.content) {
      try {
        const response = await axios.post('https://localhost:7159/api/notes', newNote);
        setNotes([...notes, response.data]);
        setFilteredNotes([...notes, response.data]);
        setNewNote({ title: '', content: '' });
      } catch (err) {
        console.error('Failed to add note:', err);
        setError('Failed to add note');
      }
    }
  };

  const handleDeleteNote = async (id) => {
    try {
      await axios.delete(`https://localhost:7159/api/notes/${id}`);
      const updatedNotes = notes.filter(note => note.id !== id);
      setNotes(updatedNotes);
      setFilteredNotes(updatedNotes);
    } catch (err) {
      console.error('Failed to delete note:', err);
      setError('Failed to delete note');
    }
  };

  return (
    <div className="notes-container">
      <h1>SuperNotes</h1>
      
      {error && <div className="error-message">{error}</div>}
      
      <div className="search-bar">
        <input
          type="text"
          value={searchQuery}
          onChange={handleSearch}
          placeholder="Search by title"
        />
      </div>

      <h2>Your Notes</h2>
      
      <div className="new-note-form">
        <input
          type="text"
          value={newNote.title}
          onChange={(e) => setNewNote({ ...newNote, title: e.target.value })}
          placeholder="Note Title"
        />
        <textarea
          value={newNote.content}
          onChange={(e) => setNewNote({ ...newNote, content: e.target.value })}
          placeholder="Note Content"
        />
        <button onClick={handleAddNote}>Add Note</button>
      </div>

      <div className="notes-list">
        {filteredNotes.map((note) => (
          <div key={note.id} className="note">
            <h4>{note.title}</h4>
            <p>{note.content}</p>
            <button onClick={() => handleDeleteNote(note.id)}>Delete</button>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Notes;

