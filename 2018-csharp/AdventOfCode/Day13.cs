using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day13 : BaseDay
{
    private readonly char[,] _tracks;
    private readonly List<Cart> _initialCarts;
    private readonly int _width;
    private readonly int _height;

    public Day13()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        _height = lines.Length;
        _width = lines.Max(line => line.Length);

        _tracks = new char[_width, _height];
        _initialCarts = new List<Cart>();

        // Parse the track layout and cart positions
        for (int y = 0; y < _height; y++)
        {
            string line = lines[y].PadRight(_width);
            for (int x = 0; x < _width; x++)
            {
                char c = line[x];

                // Check if this is a cart
                if (c == '^' || c == 'v' || c == '<' || c == '>')
                {
                    // Add the cart to our list
                    _initialCarts.Add(new Cart(x, y, c));

                    // Replace the cart with the appropriate track piece
                    c = c == '^' || c == 'v' ? '|' : '-';
                }

                _tracks[x, y] = c;
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var (collisionX, collisionY) = FindFirstCollision();
        return new($"{collisionX},{collisionY}");
    }

    public override ValueTask<string> Solve_2()
    {
        var (lastCartX, lastCartY) = FindLastRemainingCart();
        return new($"{lastCartX},{lastCartY}");
    }

    private (int x, int y) FindFirstCollision()
    {
        // Create a copy of carts for simulation
        List<Cart> carts = _initialCarts.Select(c => c.Clone()).ToList();

        while (true)
        {
            // Sort carts by position (top to bottom, left to right)
            carts = carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();

            foreach (var cart in carts)
            {
                // Move the cart
                cart.Move();

                // Check if the cart hits a curve or intersection
                char trackPiece = _tracks[cart.X, cart.Y];
                cart.FollowTrack(trackPiece);

                // Check for collision with any other cart
                foreach (var otherCart in carts)
                {
                    if (cart != otherCart && cart.X == otherCart.X && cart.Y == otherCart.Y)
                    {
                        // Collision found!
                        return (cart.X, cart.Y);
                    }
                }
            }
        }
    }

    private (int x, int y) FindLastRemainingCart()
    {
        // Create a copy of carts for simulation
        List<Cart> carts = _initialCarts.Select(c => c.Clone()).ToList();

        while (carts.Count > 1)
        {
            // Sort carts by position
            carts = carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();

            // Keep track of carts to remove
            List<Cart> collidedCarts = new List<Cart>();

            foreach (var cart in carts)
            {
                // Skip if this cart has already been involved in a collision
                if (collidedCarts.Contains(cart))
                    continue;

                // Move the cart
                cart.Move();

                // Follow track
                char trackPiece = _tracks[cart.X, cart.Y];
                cart.FollowTrack(trackPiece);

                // Check for collision with any other cart
                foreach (var otherCart in carts)
                {
                    if (cart != otherCart && !collidedCarts.Contains(otherCart) &&
                        cart.X == otherCart.X && cart.Y == otherCart.Y)
                    {
                        // Collision found - mark both carts for removal
                        collidedCarts.Add(cart);
                        collidedCarts.Add(otherCart);
                        break;
                    }
                }
            }

            // Remove all collided carts
            foreach (var cart in collidedCarts)
            {
                carts.Remove(cart);
            }
        }

        // Return the position of the last remaining cart
        return carts.Count == 1 ? (carts[0].X, carts[0].Y) : (-1, -1);
    }

    private class Cart
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public char Direction { get; private set; }
        private int _nextTurn; // 0 = left, 1 = straight, 2 = right

        public Cart(int x, int y, char direction)
        {
            X = x;
            Y = y;
            Direction = direction;
            _nextTurn = 0;
        }

        public Cart Clone()
        {
            return new Cart(X, Y, Direction) { _nextTurn = _nextTurn };
        }

        public void Move()
        {
            switch (Direction)
            {
                case '^': Y--; break;
                case 'v': Y++; break;
                case '<': X--; break;
                case '>': X++; break;
            }
        }

        public void FollowTrack(char trackPiece)
        {
            switch (trackPiece)
            {
                case '/':
                    Direction = Direction switch
                    {
                        '^' => '>',
                        'v' => '<',
                        '<' => 'v',
                        '>' => '^',
                        _ => Direction
                    };
                    break;

                case '\\':
                    Direction = Direction switch
                    {
                        '^' => '<',
                        'v' => '>',
                        '<' => '^',
                        '>' => 'v',
                        _ => Direction
                    };
                    break;

                case '+':
                    // At intersection, turn based on next turn value
                    TurnAtIntersection();
                    _nextTurn = (_nextTurn + 1) % 3;
                    break;

                    // For straight tracks, don't change direction
            }
        }

        private void TurnAtIntersection()
        {
            switch (_nextTurn)
            {
                case 0: // Turn left
                    Direction = Direction switch
                    {
                        '^' => '<',
                        'v' => '>',
                        '<' => 'v',
                        '>' => '^',
                        _ => Direction
                    };
                    break;

                case 1: // Go straight
                    // No change in direction
                    break;

                case 2: // Turn right
                    Direction = Direction switch
                    {
                        '^' => '>',
                        'v' => '<',
                        '<' => '^',
                        '>' => 'v',
                        _ => Direction
                    };
                    break;
            }
        }
    }
}