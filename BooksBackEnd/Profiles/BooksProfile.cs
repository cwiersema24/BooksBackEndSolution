﻿using AutoMapper;
using BooksBackEnd.Domain;
using BooksBackEnd.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksBackEnd.Profiles
{
    public class BooksProfile:Profile
    {
        public BooksProfile()
        {
            CreateMap<Book, BooksResponseItem>();
            CreateMap<Book, GetBookDetailsResponse>();
            CreateMap<BookCreateRequest, Book>()
                .ForMember(dest => dest.IsInInventory, opt => opt.MapFrom((x) => true))
                .ForMember(dest => dest.DateAdded, opt => opt.MapFrom((x) => DateTime.Now));
        }
    }
}
