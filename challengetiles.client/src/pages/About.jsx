//page with History of Game Creation

//css - static.css used for all text pages
import "../styles/static.css";

const About = () => {
    return (
        <div className="text">
            <h1>History Of Game</h1>
            <p>
                This is a game designed to encourage Cooperation.
                It was initially created to help children with ADHD learn to interact effectively with others.
            </p>
            <ul className="list">
                <li>
                    It is a web based version of a tile board game Created by Dr. Stephen Clarfield to use in his practice.
                    Dr. Clarfield is a PhD psychologist who has a large body of experienc in chinld psychology.
                </li>
                <li>
                    The game was developed after observing children and adolescents with ADHD
                    in a clinical setting who have issues with competition and loss.
                </li>
                <li>
                    Dr Claifield observed that children and adolescents with ADHD repeatedly selected games
                    that they were skilled at and avoided more challenging games where loss was possible.
                </li>
                <li>
                    Children and adolescents need to develop cooperative skills to function in a team environment and work successfully as adults.
                    Therefore, Dr. Clarfield developed this game to teach and encourage development of these skills
                </li>
            </ul>
        </div>
    )
}

export default About