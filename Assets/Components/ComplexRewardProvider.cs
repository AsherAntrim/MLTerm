using UnityEngine;

public class ComplexRewardProvider : SimpleRewardProvider {
    enum State {
        Reward,
        NoOp,
        Penalty
    }

    public OptionalFloat penaltyAmount;

    public Material noOpMaterial;
    public Material penaltyMaterial;
    public bool cycles;

    private Material originalMaterial;
    private State state = State.Reward;

    void Start() {
        originalMaterial = GetComponent<MeshRenderer>().material;
    }

    public override float ProvideReward() {
        return state switch {
            State.Reward => base.ProvideReward(),
            State.NoOp => 0,
            State.Penalty => penaltyAmount.useValue ? penaltyAmount.value : -rewardAmount,
            _ => throw new System.NotImplementedException()
        };
    }

    public override void PostReward() {
        switch (state) {
            case State.Reward:
                state = State.NoOp;
                break;
            case State.NoOp:
                state = State.Penalty;
                break;
            case State.Penalty when cycles:
                state = State.NoOp;
                break;
        }

        var renderer = GetComponent<MeshRenderer>();
        if (renderer) {
            switch (state) {
                case State.Reward:
                    renderer.material = originalMaterial;
                    break;
                case State.NoOp:
                    if (noOpMaterial) {
                        renderer.material = noOpMaterial;
                    }
                    break;
                case State.Penalty:
                    if (penaltyMaterial) {
                        renderer.material = penaltyMaterial;
                    }
                    break;
            }
        }
    }
}
