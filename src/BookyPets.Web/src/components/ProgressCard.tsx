import "../styles/ProgressCard.css"
import type { Progress } from "../types/Progress";

interface ProgressCardProps {
    progress: Progress
}

export default function ProgressCard({ progress }: ProgressCardProps) {

    return (
        <div className="progress-card">
            <div className="progress-info">
                <h3>{progress.currentpage}/{progress.totalpages}</h3>
            </div>
        </div>
    )
}
