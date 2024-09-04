// 공통된 데미지 계산용 인터페이스
// 현재 플레이어, 타워, 커맨드센터에서 해당 인터패이스 사용
public interface IDamageable
{
    void TakeDamage(int damage);
}