﻿using quissile.wwwapi8.Models;

namespace quissile.wwwapi8.DTO
{
    public class AlternativeDTO
    {
        public AlternativeDTO(Alternative alternative) { 
            Id = alternative.Id;
            Text = alternative.Text;
            IsAnswer = alternative.IsAnswer;
        }

        public int Id { get; set; } 
        public string Text { get; set; }
        public bool IsAnswer { get; set; }
    }
}

