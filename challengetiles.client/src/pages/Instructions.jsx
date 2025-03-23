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
        </div>
    )
}

export default Instructions