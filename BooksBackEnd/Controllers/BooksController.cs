﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using BooksBackEnd.Domain;
using BooksBackEnd.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksBackEnd.Controllers
{
    public class BooksController : ControllerBase
    {
        BooksDataContext _context;
        IMapper _mapper;
        MapperConfiguration _mapperConfig;

        public BooksController(BooksDataContext context, IMapper mapper, MapperConfiguration mapperConfig)
        {
            _context = context;
            _mapper = mapper;
            _mapperConfig = mapperConfig;
        }

        [HttpPost("books")]
        public async Task<ActionResult> AddABook([FromBody] BookCreateRequest bookToAdd)
        {
            // "Post to a Collection" (books is the collection)
            // - "I'd like to add this representation as a new resource in your collection please"
            // 1. Validate the incoming representation. Does it look good? follow the rules? etc.
            // - if NOT - send a 400 error. *maybe* tell them what they did wrong.
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }else
            {
                var book = _mapper.Map<Book>(bookToAdd);
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                var response = _mapper.Map<GetBookDetailsResponse>(book);
                return CreatedAtRoute("books#getabook", new { bookId = response.Id }, response);
            }

            // 2. Add it to the domain.
            // - turn our bookToAdd -> Book
            // - Add it to the _context.Books
            // - tell it to save the changes.
            // 3. Return:
            // 201 Created status code.
            // Location: http://localhost:1337/books/5 (birth announcement!)
            // Attach to the response a copy of whatever they would get if they followed the URL at the location header.

        }

        [HttpGet("books/{bookId:int}",Name = "books#getabook")]
       public async Task<ActionResult> GetABook(int bookId)
        {
            var book = await _context.Books
                .Where(b => b.Id == bookId && b.IsInInventory)
                .ProjectTo<GetBookDetailsResponse>(_mapperConfig)
                .SingleOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(book);
            }
        }
        [HttpGet("books")]
        public async Task<ActionResult> GetAllBooks()
        {
            var response = new GetAllBooksResponse();
            var books = await _context.Books.Where(b=>b.IsInInventory == true)
                .ProjectTo<BooksResponseItem>(_mapperConfig)
                .ToListAsync();
            response.Data = books;
            return Ok(response);
        }
    }
}
