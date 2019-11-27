public class Cracker extends Passwort
{
    private String Login = "";
    private String [] PW;

    Cracker () {
        PW = new String[Passwort.Länge];
        crack();
    }

    public void crack () {
        char anfang = 'a';
        char ende   = 'z';

        while (Login != Passwort.pw) {
            for(int i = 0; i < Passwort.Länge; i++) {
                String AbisZ = "";
                AbisZ = AbisZ + anfang;
                anfang++;
                PW[i] = AbisZ;
                System.out.println(PW[i]);
                Login = PW[0];
                for(int o = i+1; o == Passwort.Länge - 1; o++){
                    Login = Login + PW[o];
                }
            }
        }
        System.out.println("Das PW ist " + Passwort.pw);
    }

    static public void charsaddieren(){
        char kek = 'a';
        while(kek!='0'){
            System.out.println(kek);
            kek++;
        }
    }

}