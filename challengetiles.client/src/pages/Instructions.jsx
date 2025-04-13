//page with History of Game Creation

//css
import "../styles/static.css";

const Instructions = () => {
    return (
        <div className="text">
            <h1>Instructions</h1>
            <h2>Goal</h2>
            <p>
                The primary goal of this game is to work with other players.  It is about cooperation, not competition.
            </p>
            <p>
                There is one cumulative score assigned to both players. The score is calculated based on the number of tiles picked.
                Each time a player picks up a tile the score is increased. The goal is to get the lowest score.
                The better you work with others the lower your total game score.
            </p>

            <h2>Game Board</h2>
            <p>
                You will be matching tiles of the same color or number to tiles on the playing board.
            </p>
            <p>
                For Example: <br />
                <img
                    src={"../images/tilePlacementExample.png"}
                    alt="Placement Example"
                    className="placement-example"
                />
            </p>

            <h2>Profile Creation</h2>
            <p>
                Each player must have a profile.
                Once a player creates a profile then they can select their profile and begin game.
            </p>

            <h2>Game Set Up</h2>
            <p>
                Game begins by selecting the number of tiles each player will have in their hand 
                and the number of colors you want to play with. There must be a minimum of 3 anda maximum of 10 tiles. 
                There must be a minimum of 2 colors. Once these things are selected, the game can begin 
                and tiles will be be dealt to each player. After dealing to players is complete, 
                the next tile will be placed in the center of the board to begin matching. 
                The players will then decide which player would be best to go first.
            </p>

            <h2>Game Play</h2>
            <p>
                Player 1 will then place a tile that matches the center tile (either the same number or same color) 
                next to the center tile. If the player does not have a matching tile, 
                they must pick up one from the tile deck. A player will continue picking up tiles 
                until one is a valid match that can be placed on the game board
            </p>
            <p>
                Once a matching tile is placed on the game board, game play moves to the second player. 
                Game play then alternates between the two players. If an incorrect tile placement is attempted, 
                placement will be rejected.
            </p>

            <h2>Scoring</h2>
            <p>
                There is one cumulative score in this game that assigned to both players. 
                The score is increased incementally each time a tile is picked up. 
                The goal is for both players get rid of all of their tiles and achieve the lowest score.
            </p>

            <h2>Game Finish</h2>
            <p>
                The game ends when both players are out of tiles. At the end of the game the score is displayed
                and the game data will be saved for furture display on the stats page.
            </p>
        </div>
    )
}

export default Instructions