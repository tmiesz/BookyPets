{
  description = "Git replica using dotnet";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixos-26.05";
  };

  outputs =
    { self, nixpkgs }:
    let
      system = "x86_64-linux";
      pkgs = nixpkgs.legacyPackages.${system};
    in
    {
      devShells.${system}.default = pkgs.mkShell {
        packages = with pkgs; [
          dotnet-sdk_10
        ];

        shellHook = ''
          export JwtSettings__Secret="random-string-at-least-32-characters-long"
          export JwtSettings__Issuer="BookyPets"
          export JwtSettings__Audience="BookyPets"
          export JwtSettings__TokenExpirationInMinutes="60"
        '';
      };
    };
}
