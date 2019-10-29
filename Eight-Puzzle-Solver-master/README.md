# Eight-Puzzle-Solver

## Introduction

The 15-puzzle (also called Gem Puzzle, Boss Puzzle, Game of Fifteen, Mystic Square and many others) is a sliding puzzle that consists of a frame of numbered square tiles in random order with one tile missing.
This is a smaller version of it, called the eight-puzzle
It has been implemented as a Windows Presentation Foundation application in Visual Studio

## How to play

To play, simply download the .exe file in the Release folder, run it, hit randomize and try to solve it. If you'd like the computer to solve it, select your favourite algorithm and hit Solve. 
If you would like to view the project source code, download the entire repo and open the .sln file in Visual Studio. 

## Algorithms used

### Randomizing the Board

The Randomize button gives the user the option to randomly arrange the board. Note that half of the starting positions for the n-puzzle are impossible to resolve, no matter how many moves are made. Therefore, to randomize the board, the algorithm literally does twenty thousand random valid moves one at a time.

Two types of algorithms were used: uninformed search and informed search. Uninformed search has no information about how close it is to the goal. It is a brute-force algorithm that goes various paths down the search tree to find the solution. In informed search, there are heuristics that can be used to find the distance to the goal state. Commonly used heuristics for this problem include counting the number of misplaced tiles and finding the sum of the manhattan distances between each block and its position in the goal configuration. 

### Uninformed Search

#### Depth-First Search

Depth-first search (DFS) is an algorithm for traversing or searching tree or graph data structures. One starts at the root (selecting some arbitrary node as the root in the case of a graph) and explores as far as possible along each branch before backtracking.

In context of the eight-puzzle, the algorithm starts by attempting to move the current board's emply cell down until it reaches the bottom. Then left, up and finally right. It goes down one path completely before it changes moves. In some ways this is how a human would play the game: without considering different paths, going down one path and never looking back. It is therefore not surprising that the number of moves required to solve the board are usually in the tens of thousands. Solutions usually are at the end of a very deep tree.

A stack is used to keep track of the boards currently queued to be processed

#### Breadth-First Search

Breadth-first search (BFS) is an algorithm for traversing or searching tree or graph data structures. It starts at the tree root and explores the neighbor nodes first, before moving to the next level neighbors.

In context of the eight-puzzle, the algorithm tries all possible moves at every level and therefore finds the solution in far fewer steps. The downside is that it explores a lot more, at times unnecessary nodes. 

A queue is used to keep track of the boards currently queued to be processed

### Informed Search

#### Greedy Best-First Search

The Greedy Best-first search is a search algorithm which explores a graph by expanding the most promising node chosen according to a specified rule, knows as the heuristic.

Here, the algorithm would simply look at the current best board among the ones to be processed and explore that one next. And as it goes with greed, the algorithm is not guaranteed to find the best solution. Since we are only going after the local optimal solution, at times, we miss the global optimum.

Ideally, a priority queue would be used to keep track of the boards currently queued to be processed but since C# does not have such a data structure, a dictionaty is used instead. The algorithm manually goes through the available boards and chooses the best from those.

#### A* Search

A* search is a computer algorithm that is widely used in pathfinding and graph traversal, the process of plotting an efficiently directed path between multiple points, called nodes. It enjoys widespread use due to its performance and accuracy.

It is very similar to the greedy best-first search, except instead of chasing after the local optima, we seach for the global. To do so, we not only keep track of how far we are from the goal, but also the current cost. This way, if we expand to a node that is very close to the goal (and therefore would be chosen by the greedy algorithm), but took a lot of steps to get to, we might not go for that.

Once again the dictionary was used and manually searched through to find the best node.

## Solution

Once the chosen algorithm finds the solution, the application displays the number of steps taken and then directs the user to solve it. The next step is displayed on the board. It is assumed that the user will correctly take the step. After each step the user takes, the next step is shown until the board is solved. This way, the user can verify the correctness of the algorithm. I would advice not trying to solve the DFS solution, unless you have nothing else to do for a few hours.

##Enjoy